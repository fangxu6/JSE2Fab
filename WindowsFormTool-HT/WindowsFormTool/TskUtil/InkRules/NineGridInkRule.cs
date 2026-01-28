using System;
using System.Collections.Generic;
using System.Linq;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 九宫格规则
    /// 检测3x3区域内存在Fail Die时，将周围Pass Die标记为Fail
    /// 支持1-3圈迭代处理
    /// </summary>
    public class NineGridInkRule : IInkRule
    {
        public const string RULE_ID = "nine_grid_ink";
        public const string RULE_NAME = "九宫格";
        public const string DESCRIPTION = "检测3x3区域内的Fail Die，将周围Pass Die标记为Fail，支持1-3圈迭代";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { "targetBinNo", 63 },
            { "rings", 1 }
        };

        public string RuleId => RULE_ID;
        public string RuleName => RULE_NAME;
        public string Description => DESCRIPTION;
        public bool SupportsMultiRing => true;

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

            // 验证 rings
            if (!parameters.ContainsKey("rings"))
                return false;
            if (parameters["rings"] is int rings)
            {
                if (rings < 1 || rings > 3)
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
            int rings = (int)parameters["rings"];

            var result = new List<Tuple<int, int>>();
            var processed = new HashSet<Tuple<int, int>>();

            for (int ring = 1; ring <= rings; ring++)
            {
                var ringResults = GetDiesToInkInRing(matrix, ring, targetBinNo);

                foreach (var coord in ringResults)
                {
                    if (!processed.Contains(coord))
                    {
                        processed.Add(coord);
                        result.Add(coord);
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
            int rings = (int)parameters["rings"];

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 按圈数处理
            for (int ring = 1; ring <= rings; ring++)
            {
                var ringDies = GetDiesToInkInRing(matrix, ring, targetBinNo);

                int ringCount = 0;
                foreach (var coord in ringDies)
                {
                    var die = matrix[coord.Item1, coord.Item2];

                    // 跳过已经被INK的Die
                    if (die.Bin == targetBinNo)
                        continue;

                    int originalBin = die.Bin;

                    // 记录原Bin统计
                    if (!result.InkedCountByBin.ContainsKey(originalBin))
                        result.InkedCountByBin[originalBin] = 0;
                    result.InkedCountByBin[originalBin]++;

                    // 修改Die状态
                    die.Bin = targetBinNo;
                    die.Attribute = DataToExcel.DieCategory.FailDie;

                    result.InkedDies.Add(coord);
                    ringCount++;
                }

                // 记录每圈统计
                result.InkedCountByRing[ring] = ringCount;
            }

            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            result.TotalInkedCount = result.InkedDies.Count;

            return result;
        }

        /// <summary>
        /// 获取指定圈数需要INK的Die
        /// </summary>
        private List<Tuple<int, int>> GetDiesToInkInRing(DieMatrix matrix, int ring, int targetBinNo)
        {
            var result = new List<Tuple<int, int>>();

            // 扩展邻域范围：第N圈对应 (2N+1) x (2N+1) 区域
            int gridSize = 2 * ring + 1;
            int halfSize = ring;

            for (int x = 0; x < matrix.XMax; x++)
            {
                for (int y = 0; y < matrix.YMax; y++)
                {
                    var centerDie = matrix[x, y];

                    // 只处理Pass Die (Bin = 1)
                    if (centerDie.Bin != 1)
                        continue;

                    // 获取 (2N+1) x (2N+1) 区域内所有的Die
                    var gridDies = GetGridDies(matrix, x, y, halfSize);

                    // 检查区域内是否存在Fail Die（Bin != 1）
                    bool hasFailInGrid = gridDies.Any(d => d.Bin != 1);

                    if (hasFailInGrid)
                    {
                        // 检查中心Die周围一圈的Pass Die（用于第1圈）
                        // 对于多圈，扩展到 (2N+1) 区域的边缘
                        if (ring == 1)
                        {
                            // 第1圈：检查3x3区域周围的8颗Die
                            var surroundingDies = GetSurroundingDies(matrix, x, y, 1);
                            foreach (var neighbor in surroundingDies)
                            {
                                if (neighbor.Bin == 1 && !result.Contains(Tuple.Create(neighbor.X, neighbor.Y)))
                                {
                                    result.Add(Tuple.Create(neighbor.X, neighbor.Y));
                                }
                            }
                        }
                        else
                        {
                            // 第N圈：检查 (2N+1) 区域边缘的Die
                            var edgeDies = GetEdgeDies(matrix, x, y, halfSize);
                            foreach (var die in edgeDies)
                            {
                                if (die.Bin == 1 && !result.Contains(Tuple.Create(die.X, die.Y)))
                                {
                                    result.Add(Tuple.Create(die.X, die.Y));
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定中心点周围 gridSize 范围内的所有Die
        /// </summary>
        private List<DataToExcel.DieData> GetGridDies(DieMatrix matrix, int centerX, int centerY, int halfSize)
        {
            var dies = new List<DataToExcel.DieData>();

            for (int x = centerX - halfSize; x <= centerX + halfSize; x++)
            {
                for (int y = centerY - halfSize; y <= centerY + halfSize; y++)
                {
                    if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    {
                        dies.Add(matrix[x, y]);
                    }
                }
            }

            return dies;
        }

        /// <summary>
        /// 获取指定中心点周围指定圈数的边缘Die
        /// </summary>
        private List<DataToExcel.DieData> GetEdgeDies(DieMatrix matrix, int centerX, int centerY, int halfSize)
        {
            var dies = new List<DataToExcel.DieData>();

            // 上边缘
            for (int x = centerX - halfSize; x <= centerX + halfSize; x++)
            {
                int y = centerY - halfSize;
                if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    dies.Add(matrix[x, y]);
            }

            // 下边缘
            for (int x = centerX - halfSize; x <= centerX + halfSize; x++)
            {
                int y = centerY + halfSize;
                if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    dies.Add(matrix[x, y]);
            }

            // 左边缘（排除上下角落）
            for (int y = centerY - halfSize + 1; y <= centerY + halfSize - 1; y++)
            {
                int x = centerX - halfSize;
                if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    dies.Add(matrix[x, y]);
            }

            // 右边缘（排除上下角落）
            for (int y = centerY - halfSize + 1; y <= centerY + halfSize - 1; y++)
            {
                int x = centerX + halfSize;
                if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    dies.Add(matrix[x, y]);
            }

            return dies;
        }

        /// <summary>
        /// 获取中心点周围指定半径的邻域Die
        /// </summary>
        private List<DataToExcel.DieData> GetSurroundingDies(DieMatrix matrix, int centerX, int centerY, int radius)
        {
            var dies = new List<DataToExcel.DieData>();

            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    // 跳过中心点本身
                    if (x == centerX && y == centerY)
                        continue;

                    if (x >= 0 && x < matrix.XMax && y >= 0 && y < matrix.YMax)
                    {
                        dies.Add(matrix[x, y]);
                    }
                }
            }

            return dies;
        }
    }
}
