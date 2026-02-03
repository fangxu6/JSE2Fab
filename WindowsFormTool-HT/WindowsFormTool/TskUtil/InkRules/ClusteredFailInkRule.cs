using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// 团簇Fail扩散规则
    /// </summary>
    public class ClusteredFailInkRule : IInkRule
    {
        public const string RULE_ID = "clustered_fail_ink";
        public const string RULE_NAME = "团簇Fail扩散";
        public const string DESCRIPTION = "检测Fail团簇并标记周边Pass";

        private static readonly Dictionary<string, object> DefaultParameters = new Dictionary<string, object>
        {
            { InkRuleParameters.TargetBinNo, 63 },
            { InkRuleParameters.MinClusterSize, 10 }
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

            if (!parameters.ContainsKey(InkRuleParameters.MinClusterSize))
                return false;
            if (!(parameters[InkRuleParameters.MinClusterSize] is int minCluster) || minCluster < 10)
                return false;

            return true;
        }

        public List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters)
        {
            if (!ValidateParameters(parameters))
                throw new ArgumentException("参数验证失败");

            int minCluster = (int)parameters[InkRuleParameters.MinClusterSize];
            var visited = new bool[matrix.XMax, matrix.YMax];
            var inkTargets = new HashSet<Tuple<int, int>>();

            for (int x = 0; x < matrix.XMax; x++)
            {
                for (int y = 0; y < matrix.YMax; y++)
                {
                    if (visited[x, y])
                        continue;

                    var die = matrix[x, y];
                    if (!IsFailCandidate(die))
                        continue;

                    var cluster = new List<Tuple<int, int>>();
                    FloodFillFailCluster(matrix, x, y, visited, cluster);

                    if (cluster.Count < minCluster)
                        continue;

                    foreach (var coord in cluster)
                    {
                        foreach (var neighborCoord in GetEightNeighbors(matrix, coord.Item1, coord.Item2))
                        {
                            var neighbor = matrix[neighborCoord.Item1, neighborCoord.Item2];
                            if (IsPassDie(neighbor))
                                inkTargets.Add(neighborCoord);
                        }
                    }
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

        private void FloodFillFailCluster(DieMatrix matrix, int startX, int startY, bool[,] visited,
            List<Tuple<int, int>> cluster)
        {
            var queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(Tuple.Create(startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                var coord = queue.Dequeue();
                int x = coord.Item1;
                int y = coord.Item2;

                cluster.Add(coord);

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx < 0 || ny < 0 || nx >= matrix.XMax || ny >= matrix.YMax)
                            continue;
                        if (visited[nx, ny])
                            continue;

                        var neighbor = matrix[nx, ny];
                        if (!IsFailCandidate(neighbor))
                            continue;

                        visited[nx, ny] = true;
                        queue.Enqueue(Tuple.Create(nx, ny));
                    }
                }
            }
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
