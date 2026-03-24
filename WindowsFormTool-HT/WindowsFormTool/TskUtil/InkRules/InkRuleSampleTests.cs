using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    internal static class InkRuleSampleTests
    {
        public static List<string> RunBasicChecks()
        {
            var failures = new List<string>();

            if (!CheckEnclosedPassRule())
                failures.Add("EnclosedPassInkRule: expected enclosed pass to be inked.");

            if (!CheckLineBlobRule())
                failures.Add("LineBlobInkRule: expected neighbor pass to be inked.");

            if (!CheckClusteredFailRule())
                failures.Add("ClusteredFailInkRule: expected neighbor pass to be inked.");

            if (!CheckGdbcThresholdRule())
                failures.Add("GdbcNineGridThresholdInkRule: expected center pass to be inked.");

            if (!CheckNineGridPatternRule())
                failures.Add("NineGridPatternInkRule: expected center pass to be inked.");

            return failures;
        }

        private static bool CheckNineGridPatternRule()
        {
            // 创建 3x3 矩阵，中心为 Pass，周围全为 Fail
            var matrix = CreateMatrix(3, 3, DieCategory.FailDie, 2);
            SetDie(matrix, 1, 1, DieCategory.PassDie, 1);

            var rule = new NineGridPatternInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);

            // 验证中心点被识别
            bool success = ContainsCoord(result, 1, 1);

            // 属性优先验证：即使 Bin 与常规值不一致，也应基于 Attribute 判定 Pass/Fail
            var attributeFirstMatrix = CreateMatrix(3, 3, DieCategory.FailDie, 1);
            SetDie(attributeFirstMatrix, 1, 1, DieCategory.PassDie, 2);
            var attributeFirstResult = rule.Preview(attributeFirstMatrix, parameters);
            success = success && ContainsCoord(attributeFirstResult, 1, 1);

            // 验证边缘点不触发（即使被包围，但在 3x3 矩阵中边缘点没有 8 个邻居）
            // 注意：NineGridPatternInkRule.Preview 循环是从 1 到 Max-1，所以边缘点根本不会被检查。

            return success;
        }

        private static bool CheckEnclosedPassRule()
        {
            var matrix = CreateMatrix(5, 5, DieCategory.FailDie, 2);
            SetDie(matrix, 2, 2, DieCategory.PassDie, 1);

            var rule = new EnclosedPassInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);

            return ContainsCoord(result, 2, 2);
        }

        private static bool CheckLineBlobRule()
        {
            var matrix = CreateMatrix(10, 5, DieCategory.PassDie, 1);
            for (int x = 2; x <= 7; x++)
                SetDie(matrix, x, 2, DieCategory.FailDie, 2);

            var rule = new LineBlobInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);
            var multiRingParameters = new Dictionary<string, object>(parameters);
            multiRingParameters[InkRuleParameters.Rings] = 2;
            var multiRingResult = rule.Preview(matrix, multiRingParameters);

            return parameters.ContainsKey(InkRuleParameters.Rings) &&
                   (int)parameters[InkRuleParameters.Rings] == 1 &&
                   ContainsCoord(result, 2, 1) &&
                   multiRingResult.Count > result.Count;
        }

        private static bool CheckClusteredFailRule()
        {
            var matrix = CreateMatrix(9, 7, DieCategory.PassDie, 1);
            for (int x = 3; x <= 6; x++)
            {
                for (int y = 2; y <= 4; y++)
                {
                    SetDie(matrix, x, y, DieCategory.FailDie, 2);
                }
            }

            var rule = new ClusteredFailInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);
            var multiRingParameters = new Dictionary<string, object>(parameters);
            multiRingParameters[InkRuleParameters.Rings] = 2;
            var multiRingResult = rule.Preview(matrix, multiRingParameters);

            return parameters.ContainsKey(InkRuleParameters.Rings) &&
                   (int)parameters[InkRuleParameters.Rings] == 1 &&
                   ContainsCoord(result, 2, 2) &&
                   multiRingResult.Count > result.Count;
        }

        private static bool CheckGdbcThresholdRule()
        {
            var matrix = CreateMatrix(3, 3, DieCategory.PassDie, 1);
            SetDie(matrix, 0, 0, DieCategory.FailDie, 2);
            SetDie(matrix, 0, 1, DieCategory.FailDie, 2);
            SetDie(matrix, 0, 2, DieCategory.FailDie, 2);
            SetDie(matrix, 1, 0, DieCategory.FailDie, 2);

            var rule = new GdbcNineGridThresholdInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);

            return ContainsCoord(result, 1, 1);
        }

        private static DieMatrix CreateMatrix(int width, int height, DieCategory attribute, int bin)
        {
            var matrix = new DieMatrix(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetDie(matrix, x, y, attribute, bin);
                }
            }

            return matrix;
        }

        private static void SetDie(DieMatrix matrix, int x, int y, DieCategory attribute, int bin)
        {
            var die = matrix[x, y];
            die.Attribute = attribute;
            die.Bin = bin;
            die.X = x;
            die.Y = y;
        }

        private static bool ContainsCoord(List<System.Tuple<int, int>> coords, int x, int y)
        {
            foreach (var coord in coords)
            {
                if (coord.Item1 == x && coord.Item2 == y)
                    return true;
            }
            return false;
        }
    }
}
