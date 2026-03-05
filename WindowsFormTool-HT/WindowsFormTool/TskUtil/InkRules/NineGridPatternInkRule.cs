using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 九宫格围点规则 (NineGrid Pattern Ink Rule)
    /// 当一颗 Pass 芯片的四周 8 颗芯片全为 Fail 的情况下需要 ink 为 Fail。
    /// 严格模式：必须拥有完整的 8 个邻域且全部为 Fail。
    /// </summary>
    public class NineGridPatternInkRule : IInkRule
    {
        public const string RULE_ID = "nine_grid_pattern_ink";
        public const string RULE_NAME = "九宫格围点 (NineGrid Pattern)";
        public const string DESCRIPTION = "检测被8颗Fail Die完全包围的Pass Die，将其标记为Fail";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { "targetBinNo", 63 }
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

            return true;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            var result = new List<Tuple<int, int>>();

            // 遍历矩阵，跳过边缘以确保 8 邻域存在
            for (int x = 1; x < matrix.XMax - 1; x++)
            {
                for (int y = 1; y < matrix.YMax - 1; y++)
                {
                    var die = matrix[x, y];

                    // 只处理 Pass Die
                    if (!IsPassDie(die))
                        continue;

                    // 检查 8 邻域
                    if (IsSurroundedByFail(matrix, x, y))
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
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var inkingDies = Preview(matrix, parameters);

            foreach (var coord in inkingDies)
            {
                var die = matrix[coord.Item1, coord.Item2];
                int originalBin = die.Bin;

                // 记录统计
                if (!result.InkedCountByBin.ContainsKey(originalBin))
                    result.InkedCountByBin[originalBin] = 0;
                result.InkedCountByBin[originalBin]++;

                // 修改状态
                die.Bin = targetBinNo;
                die.Attribute = DieCategory.FailDie;

                result.InkedDies.Add(coord);
            }

            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            result.TotalInkedCount = result.InkedDies.Count;
            result.Success = true;

            return result;
        }

        /// <summary>
        /// 检查指定坐标的 Die 是否被 8 个 Fail Die 包围
        /// </summary>
        private bool IsSurroundedByFail(DieMatrix matrix, int x, int y)
        {
            // 定义 8 邻域偏移量
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                // 如果超出边界（理论上 Preview 循环已经过滤，但这里做双重保险）
                if (nx < 0 || nx >= matrix.XMax || ny < 0 || ny >= matrix.YMax)
                    return false;

                var neighbor = matrix[nx, ny];
                // 邻域必须全部为 Fail Die
                if (!IsFailDie(neighbor))
                    return false;
            }

            return true;
        }

        private bool IsPassDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.PassDie;
        }

        private bool IsFailDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.FailDie;
        }
    }
}
