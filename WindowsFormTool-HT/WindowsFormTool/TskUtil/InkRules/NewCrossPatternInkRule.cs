using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 新十字围点规则
    /// 检测水平或垂直连续Pass线段被Fail/Mark/Skip2包围的情况
    /// </summary>
    public class NewCrossPatternInkRule : IInkRule
    {
        public const string RULE_ID = "new_cross_pattern_ink";
        public const string RULE_NAME = "新十字围点";
        public const string DESCRIPTION = "检测被Fail/Mark/Skip2包围的连续Pass线段并标记为Fail";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { InkRuleParameters.TargetBinNo, 63 }
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

            if (!(parameters[InkRuleParameters.TargetBinNo] is int targetBinNo))
                return false;

            return targetBinNo >= 1 && targetBinNo <= 255;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            var targets = new HashSet<Tuple<int, int>>();

            AddHorizontalTargets(matrix, targets);
            AddVerticalTargets(matrix, targets);

            return new List<Tuple<int, int>>(targets);
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

        private void AddHorizontalTargets(DieMatrix matrix, HashSet<Tuple<int, int>> targets)
        {
            for (int y = 0; y < matrix.YMax; y++)
            {
                int x = 0;
                while (x < matrix.XMax)
                {
                    if (!IsPassDie(matrix[x, y]))
                    {
                        x++;
                        continue;
                    }

                    int startX = x;
                    while (x < matrix.XMax && IsPassDie(matrix[x, y]))
                        x++;

                    int endX = x - 1;
                    int length = endX - startX + 1;
                    if (length >= 2 && IsHorizontalEnclosed(matrix, y, startX, endX))
                    {
                        for (int ix = startX; ix <= endX; ix++)
                        {
                            targets.Add(Tuple.Create(ix, y));
                        }
                    }
                }
            }
        }

        private void AddVerticalTargets(DieMatrix matrix, HashSet<Tuple<int, int>> targets)
        {
            for (int x = 0; x < matrix.XMax; x++)
            {
                int y = 0;
                while (y < matrix.YMax)
                {
                    if (!IsPassDie(matrix[x, y]))
                    {
                        y++;
                        continue;
                    }

                    int startY = y;
                    while (y < matrix.YMax && IsPassDie(matrix[x, y]))
                        y++;

                    int endY = y - 1;
                    int length = endY - startY + 1;
                    if (length >= 2 && IsVerticalEnclosed(matrix, x, startY, endY))
                    {
                        for (int iy = startY; iy <= endY; iy++)
                        {
                            targets.Add(Tuple.Create(x, iy));
                        }
                    }
                }
            }
        }

        private bool IsHorizontalEnclosed(DieMatrix matrix, int y, int startX, int endX)
        {
            if (y <= 0 || y >= matrix.YMax - 1)
                return false;
            if (startX <= 0 || endX >= matrix.XMax - 1)
                return false;

            for (int x = startX; x <= endX; x++)
            {
                if (!IsSurroundDie(matrix[x, y - 1]) || !IsSurroundDie(matrix[x, y + 1]))
                    return false;
            }

            return IsSurroundDie(matrix[startX - 1, y]) && IsSurroundDie(matrix[endX + 1, y]);
        }

        private bool IsVerticalEnclosed(DieMatrix matrix, int x, int startY, int endY)
        {
            if (x <= 0 || x >= matrix.XMax - 1)
                return false;
            if (startY <= 0 || endY >= matrix.YMax - 1)
                return false;

            for (int y = startY; y <= endY; y++)
            {
                if (!IsSurroundDie(matrix[x - 1, y]) || !IsSurroundDie(matrix[x + 1, y]))
                    return false;
            }

            return IsSurroundDie(matrix[x, startY - 1]) && IsSurroundDie(matrix[x, endY + 1]);
        }

        private bool IsPassDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.PassDie;
        }

        private bool IsSurroundDie(DieData die)
        {
            if (die == null)
                return false;

            return die.Attribute == DieCategory.FailDie ||
                   die.Attribute == DieCategory.MarkDie ||
                   die.Attribute == DieCategory.SkipDie2;
        }
    }
}
