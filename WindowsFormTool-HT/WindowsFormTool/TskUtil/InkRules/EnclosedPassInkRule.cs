using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 被Fail包围的Pass（岛状）规则
    /// </summary>
    public class EnclosedPassInkRule : IInkRule
    {
        public const string RULE_ID = "enclosed_pass_ink";
        public const string RULE_NAME = "被Fail包围的Pass";
        public const string DESCRIPTION = "识别被Fail/Mark/Skip2包围的Pass岛状区域并标记为Fail";

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

            if (parameters[InkRuleParameters.TargetBinNo] is int targetBinNo)
            {
                return targetBinNo >= 1 && targetBinNo <= 255;
            }

            return false;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            var enclosedRegions = GetEnclosedPassRegions(matrix);
            var inkingRegions = ExcludeLargestRegion(enclosedRegions);
            return FlattenRegions(inkingRegions);
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

            var enclosedRegions = GetEnclosedPassRegions(matrix);
            var inkingRegions = ExcludeLargestRegion(enclosedRegions);
            var inkingDies = FlattenRegions(inkingRegions);

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

        private List<PassRegionInfo> GetEnclosedPassRegions(DieMatrix matrix)
        {
            var regions = GetPassRegions(matrix);
            var enclosed = new List<PassRegionInfo>();

            foreach (var region in regions)
            {
                if (region.ReachesBoundary)
                    continue;

                if (region.BoundaryHasOther)
                    continue;

                enclosed.Add(region);
            }

            return enclosed;
        }

        private List<PassRegionInfo> ExcludeLargestRegion(List<PassRegionInfo> regions)
        {
            if (regions.Count <= 1)
                return regions;

            int maxIndex = 0;
            int maxSize = regions[0].Cells.Count;

            for (int i = 1; i < regions.Count; i++)
            {
                int size = regions[i].Cells.Count;
                if (size > maxSize)
                {
                    maxSize = size;
                    maxIndex = i;
                }
            }

            var filtered = new List<PassRegionInfo>(regions.Count - 1);
            for (int i = 0; i < regions.Count; i++)
            {
                if (i == maxIndex)
                    continue;

                filtered.Add(regions[i]);
            }

            return filtered;
        }

        private List<Tuple<int, int>> FlattenRegions(List<PassRegionInfo> regions)
        {
            var result = new List<Tuple<int, int>>();

            foreach (var region in regions)
            {
                foreach (var cell in region.Cells)
                {
                    result.Add(Tuple.Create(cell.Item1, cell.Item2));
                }
            }

            return result;
        }

        private List<PassRegionInfo> GetPassRegions(DieMatrix matrix)
        {
            var regions = new List<PassRegionInfo>();
            var visited = new bool[matrix.XMax, matrix.YMax];

            for (int x = 0; x < matrix.XMax; x++)
            {
                for (int y = 0; y < matrix.YMax; y++)
                {
                    if (visited[x, y])
                        continue;

                    var die = matrix[x, y];
                    if (!IsPassDie(die))
                        continue;

                    var region = new PassRegionInfo();
                    var queue = new Queue<Tuple<int, int>>();
                    queue.Enqueue(Tuple.Create(x, y));

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        int cx = current.Item1;
                        int cy = current.Item2;

                        if (visited[cx, cy])
                            continue;

                        visited[cx, cy] = true;
                        region.Cells.Add(current);

                        EvaluateNeighbor(matrix, cx - 1, cy, region, queue, visited);
                        EvaluateNeighbor(matrix, cx + 1, cy, region, queue, visited);
                        EvaluateNeighbor(matrix, cx, cy - 1, region, queue, visited);
                        EvaluateNeighbor(matrix, cx, cy + 1, region, queue, visited);
                    }

                    regions.Add(region);
                }
            }

            return regions;
        }

        private void EvaluateNeighbor(DieMatrix matrix, int x, int y, PassRegionInfo region,
            Queue<Tuple<int, int>> queue, bool[,] visited)
        {
            if (x < 0 || y < 0 || x >= matrix.XMax || y >= matrix.YMax)
            {
                region.ReachesBoundary = true;
                return;
            }

            if (visited[x, y])
                return;

            var neighbor = matrix[x, y];
            if (IsPassDie(neighbor))
            {
                queue.Enqueue(Tuple.Create(x, y));
                return;
            }

            if (IsFailDie(neighbor))
            {
                region.BoundaryHasFail = true;
            }
            else if (IsMarkOrSkip2(neighbor))
            {
                region.BoundaryHasMark = true;
            }
            else
            {
                region.BoundaryHasOther = true;
            }
        }

        private bool IsFailDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.FailDie;
        }

        private bool IsMarkOrSkip2(DieData die)
        {
            return die != null &&
                   (die.Attribute == DieCategory.MarkDie ||
                    die.Attribute == DieCategory.SkipDie2);
        }

        private bool IsPassDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.PassDie;
        }

        private class PassRegionInfo
        {
            public List<Tuple<int, int>> Cells { get; } = new List<Tuple<int, int>>();
            public bool ReachesBoundary { get; set; }
            public bool BoundaryHasFail { get; set; }
            public bool BoundaryHasMark { get; set; }
            public bool BoundaryHasOther { get; set; }
        }

    }
}
