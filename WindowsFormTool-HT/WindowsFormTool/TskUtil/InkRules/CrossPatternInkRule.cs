using System;
using System.Collections.Generic;
using System.Linq;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 十字围点规则
    /// 模式1：Pass Die上下左右四颗均为Fail Die → Ink为指定Bin
    /// 模式2：Pass Die上下左右有1-3颗Mark Die，其余为Fail Die → Ink为指定Bin
    /// </summary>
    public class CrossPatternInkRule : IInkRule
    {
        public const string RULE_ID = "cross_pattern_ink";
        public const string RULE_NAME = "十字围点";
        public const string DESCRIPTION = "检测被Fail Die包围的Pass Die，将其标记为Fail";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { "targetBinNo", 63 },
            { "mode", CrossPatternMode.PureFailSurround }
        };

        public string RuleId => RULE_ID;
        public string RuleName => RULE_NAME;
        public string Description => DESCRIPTION;
        public bool SupportsMultiRing => false;

        public Dictionary<string, object> GetDefaultParameters()
        {
            return new Dictionary<string, object>(DefaultParameters);
        }

        public bool ValidateParameters(Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return false;

            // 验证 targetBinNo
            if (!parameters.ContainsKey("targetBinNo"))
                return false;
            if (parameters["targetBinNo"] is int targetBinNo)
            {
                if (targetBinNo < 1 || targetBinNo > 255)
                    return false;
            }
            else
            {
                return false;
            }

            // 验证 mode
            if (!parameters.ContainsKey("mode"))
                return false;
            if (parameters["mode"] is int mode)
            {
                if (mode != 1 && mode != 2)
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            int targetBinNo = (int)parameters["targetBinNo"];
            int mode = (int)parameters["mode"];

            var result = new List<Tuple<int, int>>();

            for (int x = 0; x < matrix.XMax; x++)
            {
                for (int y = 0; y < matrix.YMax; y++)
                {
                    var die = matrix[x, y];

                    // 只处理Pass Die (Bin = 1)
                    if (die.Bin != 1)
                        continue;

                    // 获取四邻域Die
                    var neighbors = GetNeighbors(matrix, x, y);
                    if (neighbors.Count < 4)
                        continue; // 边缘Die跳过

                    bool shouldInk = ShouldInk(neighbors, mode);
                    if (shouldInk)
                    {
                        result.Add(Tuple.Create(x, y));
                    }
                }
            }

            return result;
        }

        public InkRuleResult Apply(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            var result = new InkRuleResult
            {
                RuleId = RuleId,
                RuleName = RuleName,
                Parameters = parameters
            };

            if (!ValidateParameters(parameters))
            {
                result.Success = false;
                result.ErrorMessage = "参数验证失败";
                return result;
            }

            int targetBinNo = (int)parameters["targetBinNo"];
            int mode = (int)parameters["mode"];

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var inkingDies = Preview(matrix, parameters);

            foreach (var coord in inkingDies)
            {
                var die = matrix[coord.Item1, coord.Item2];
                int originalBin = die.Bin;

                // 记录原Bin统计
                if (!result.InkedCountByBin.ContainsKey(originalBin))
                    result.InkedCountByBin[originalBin] = 0;
                result.InkedCountByBin[originalBin]++;

                // 修改Die状态
                die.Bin = targetBinNo;
                die.Attribute = DataToExcel.DieCategory.FailDie;

                result.InkedDies.Add(coord);
            }

            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            result.TotalInkedCount = result.InkedDies.Count;

            return result;
        }

        /// <summary>
        /// 获取四邻域Die
        /// </summary>
        private List<DataToExcel.DieData> GetNeighbors(DieMatrix matrix, int x, int y)
        {
            var neighbors = new List<DataToExcel.DieData>();

            // 上 (y-1)
            if (y > 0)
                neighbors.Add(matrix[x, y - 1]);

            // 下 (y+1)
            if (y < matrix.YMax - 1)
                neighbors.Add(matrix[x, y + 1]);

            // 左 (x-1)
            if (x > 0)
                neighbors.Add(matrix[x - 1, y]);

            // 右 (x+1)
            if (x < matrix.XMax - 1)
                neighbors.Add(matrix[x + 1, y]);

            return neighbors;
        }

        /// <summary>
        /// 判断是否应该INK
        /// </summary>
        private bool ShouldInk(List<DataToExcel.DieData> neighbors, int mode)
        {
            if (neighbors.Count != 4)
                return false;

            int failCount = 0;
            int markCount = 0;

            foreach (var neighbor in neighbors)
            {
                // Bin != 1 表示 Fail（包含各种Fail类型）
                if (neighbor.Bin != 1)
                {
                    failCount++;
                    if (neighbor.Attribute == DataToExcel.DieCategory.MarkDie)
                    {
                        markCount++;
                    }
                }
            }

            if (mode == (int)CrossPatternMode.PureFailSurround)
            {
                // 模式1：四颗均为Fail（无Mark）
                return failCount == 4 && markCount == 0;
            }
            else if (mode == (int)CrossPatternMode.MarkFailSurround)
            {
                // 模式2：1-3颗Mark + 其余Fail
                return failCount == 4 && markCount >= 1 && markCount <= 3;
            }

            return false;
        }
    }
}
