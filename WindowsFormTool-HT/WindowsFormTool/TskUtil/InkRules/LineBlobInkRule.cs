using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 线状Fail扩散规则
    /// </summary>
    public class LineBlobInkRule : IInkRule
    {
        public const string RULE_ID = "line_blob_ink";
        public const string RULE_NAME = "线状Fail扩散";
        public const string DESCRIPTION = "检测水平或垂直Fail线段并标记周边Pass";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { InkRuleParameters.TargetBinNo, 63 },
            { InkRuleParameters.MinLineLength, 6 }
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

            if (!parameters.ContainsKey(InkRuleParameters.MinLineLength))
                return false;
            if (!(parameters[InkRuleParameters.MinLineLength] is int minLength) || minLength < 6)
                return false;

            return true;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            int minLength = (int)parameters[InkRuleParameters.MinLineLength];
            var lineDies = new HashSet<Tuple<int, int>>();

            // 水平线段
            for (int y = 0; y < matrix.YMax; y++)
            {
                int x = 0;
                while (x < matrix.XMax)
                {
                    if (!IsFailCandidate(matrix[x, y]))
                    {
                        x++;
                        continue;
                    }

                    int startX = x;
                    while (x < matrix.XMax && IsFailCandidate(matrix[x, y]))
                        x++;

                    int length = x - startX;
                    if (length >= minLength)
                    {
                        for (int ix = startX; ix < x; ix++)
                        {
                            lineDies.Add(Tuple.Create(ix, y));
                        }
                    }
                }
            }

            // 垂直线段
            for (int x = 0; x < matrix.XMax; x++)
            {
                int y = 0;
                while (y < matrix.YMax)
                {
                    if (!IsFailCandidate(matrix[x, y]))
                    {
                        y++;
                        continue;
                    }

                    int startY = y;
                    while (y < matrix.YMax && IsFailCandidate(matrix[x, y]))
                        y++;

                    int length = y - startY;
                    if (length >= minLength)
                    {
                        for (int iy = startY; iy < y; iy++)
                        {
                            lineDies.Add(Tuple.Create(x, iy));
                        }
                    }
                }
            }

            var inkTargets = new HashSet<Tuple<int, int>>();
            foreach (var coord in lineDies)
            {
                foreach (var neighborCoord in GetEightNeighbors(matrix, coord.Item1, coord.Item2))
                {
                    var neighbor = matrix[neighborCoord.Item1, neighborCoord.Item2];
                    if (IsPassDie(neighbor))
                        inkTargets.Add(neighborCoord);
                }
            }

            return new List<Tuple<int, int>>(inkTargets);
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

        private bool IsFailCandidate(DieData die)
        {
            if (die == null)
                return false;

            if (die.Attribute == DieCategory.PassDie)
                return false;

            return !IsMarkOrSkip(die.Attribute);
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

        private List<Tuple<int, int>> GetEightNeighbors(DieMatrix matrix, int x, int y)
        {
            var neighbors = new List<Tuple<int, int>>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < matrix.XMax && ny >= 0 && ny < matrix.YMax)
                        neighbors.Add(Tuple.Create(nx, ny));
                }
            }

            return neighbors;
        }
    }
}
