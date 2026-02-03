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
        public const string DESCRIPTION = "识别被Fail包围的Pass岛状区域并标记为Fail";

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

            var result = new List<Tuple<int, int>>();
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

                    var region = new List<Tuple<int, int>>();
                    bool touchesBoundary = FloodFillPassRegion(matrix, x, y, visited, region);

                    if (!touchesBoundary)
                        result.AddRange(region);
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

        private bool FloodFillPassRegion(DieMatrix matrix, int startX, int startY, bool[,] visited,
            List<Tuple<int, int>> region)
        {
            bool touchesBoundary = false;
            var queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(Tuple.Create(startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                var coord = queue.Dequeue();
                int x = coord.Item1;
                int y = coord.Item2;

                region.Add(coord);

                // 检查是否接触到真正的wafer边界（忽略Mark区域）
                if (IsTouchingRealBoundary(matrix, x, y))
                    touchesBoundary = true;

                TryVisitNeighbor(matrix, x - 1, y, visited, queue);
                TryVisitNeighbor(matrix, x + 1, y, visited, queue);
                TryVisitNeighbor(matrix, x, y - 1, visited, queue);
                TryVisitNeighbor(matrix, x, y + 1, visited, queue);
            }

            return touchesBoundary;
        }

        /// <summary>
        /// 检查指定位置是否接触到真正的wafer边界（穿过Mark区域检测）
        /// </summary>
        private bool IsTouchingRealBoundary(DieMatrix matrix, int x, int y)
        {
            // 检查四个方向，如果任一方向穿过Mark后到达边界或null，则认为接触边界
            return IsBoundaryInDirection(matrix, x, y, -1, 0) ||  // 左
                   IsBoundaryInDirection(matrix, x, y, 1, 0) ||   // 右
                   IsBoundaryInDirection(matrix, x, y, 0, -1) ||  // 上
                   IsBoundaryInDirection(matrix, x, y, 0, 1);     // 下
        }

        /// <summary>
        /// 检查指定方向是否到达真正的边界（穿过Mark区域）
        /// </summary>
        private bool IsBoundaryInDirection(DieMatrix matrix, int x, int y, int dx, int dy)
        {
            int currentX = x + dx;
            int currentY = y + dy;

            // 沿着指定方向前进，穿过Mark区域
            while (currentX >= 0 && currentX < matrix.XMax && 
                   currentY >= 0 && currentY < matrix.YMax)
            {
                var die = matrix[currentX, currentY];
                
                // 如果遇到null，说明到达边界
                if (die == null)
                    return true;

                // 如果遇到Mark，继续前进穿过它
                if (die.Attribute == DieCategory.MarkDie || die.Attribute == DieCategory.SkipDie2)
                {
                    currentX += dx;
                    currentY += dy;
                    continue;
                }

                // 如果遇到Pass或Fail，说明没有到达边界
                return false;
            }

            // 超出矩阵范围，说明到达边界
            return true;
        }

        private void TryVisitNeighbor(DieMatrix matrix, int x, int y, bool[,] visited, Queue<Tuple<int, int>> queue)
        {
            if (x < 0 || y < 0 || x >= matrix.XMax || y >= matrix.YMax)
                return;
            if (visited[x, y])
                return;

            var die = matrix[x, y];
            if (!IsPassDie(die))
                return;

            visited[x, y] = true;
            queue.Enqueue(Tuple.Create(x, y));
        }

        private bool IsPassDie(DieData die)
        {
            return die != null && die.Attribute == DieCategory.PassDie;
        }
    }
}