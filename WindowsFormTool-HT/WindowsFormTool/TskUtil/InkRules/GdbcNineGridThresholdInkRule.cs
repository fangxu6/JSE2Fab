using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// GDBC九宫格围点阈值规则
    /// </summary>
    public class GdbcNineGridThresholdInkRule : IInkRule
    {
        public const string RULE_ID = "gdbc_nine_grid_threshold_ink";
        public const string RULE_NAME = "GDBC九宫格围点";
        public const string DESCRIPTION = "按周边Fail占比阈值标记Pass";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { InkRuleParameters.TargetBinNo, 63 },
            { InkRuleParameters.Threshold, 0.5 }
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

            if (!parameters.ContainsKey(InkRuleParameters.TargetBinNo))
                return false;
            if (!(parameters[InkRuleParameters.TargetBinNo] is int targetBinNo) || targetBinNo < 1 || targetBinNo > 255)
                return false;

            if (!parameters.ContainsKey(InkRuleParameters.Threshold))
                return false;

            if (!TryGetThreshold(parameters[InkRuleParameters.Threshold], out double threshold))
                return false;

            return threshold >= 0.0 && threshold <= 1.0;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            double threshold = Convert.ToDouble(parameters[InkRuleParameters.Threshold]);
            var result = new List<Tuple<int, int>>();

            for (int x = 0; x < matrix.XMax; x++)
            {
                for (int y = 0; y < matrix.YMax; y++)
                {
                    var die = matrix[x, y];
                    if (!IsPassDie(die))
                        continue;

                    int passCount = 0;
                    int failCount = 0;

                    foreach (var neighbor in GetEightNeighbors(matrix, x, y))
                    {
                        if (IsMarkOrSkip(neighbor.Attribute))
                            continue;

                        if (neighbor.Attribute == DieCategory.PassDie)
                            passCount++;
                        else
                            failCount++;
                    }

                    int total = passCount + failCount;
                    if (total == 0)
                        continue;

                    double ratio = (double)failCount / total;
                    if (ratio >= threshold)
                        result.Add(Tuple.Create(x, y));
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

            int targetBinNo = (int)parameters[InkRuleParameters.TargetBinNo];
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var inkingDies = Preview(matrix, parameters);
            foreach (var coord in inkingDies)
            {
                var die = matrix[coord.Item1, coord.Item2];
                int originalBin = die.Bin;

                if (!result.InkedCountByBin.ContainsKey(originalBin))
                    result.InkedCountByBin[originalBin] = 0;
                result.InkedCountByBin[originalBin]++;

                die.Bin = targetBinNo;
                die.Attribute = DieCategory.FailDie;
                result.InkedDies.Add(coord);
            }

            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            result.TotalInkedCount = result.InkedDies.Count;

            return result;
        }

        private bool TryGetThreshold(object value, out double threshold)
        {
            if (value is double doubleValue)
            {
                threshold = doubleValue;
                return true;
            }

            if (value is decimal decimalValue)
            {
                threshold = (double)decimalValue;
                return true;
            }

            if (value is float floatValue)
            {
                threshold = floatValue;
                return true;
            }

            threshold = 0.0;
            return false;
        }

        private bool IsPassDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.PassDie;
        }

        private bool IsMarkOrSkip(DieCategory attribute)
        {
            return attribute == DieCategory.MarkDie ||
                   attribute == DieCategory.SkipDie ||
                   attribute == DieCategory.SkipDie2;
        }

        private List<DieData> GetEightNeighbors(DieMatrix matrix, int x, int y)
        {
            var neighbors = new List<DieData>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < matrix.XMax && ny >= 0 && ny < matrix.YMax)
                        neighbors.Add(matrix[nx, ny]);
                }
            }

            return neighbors;
        }
    }
}
