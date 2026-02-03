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

            return failures;
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
            var matrix = CreateMatrix(8, 3, DieCategory.PassDie, 1);
            for (int x = 1; x <= 6; x++)
                SetDie(matrix, x, 1, DieCategory.FailDie, 2);

            var rule = new LineBlobInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);

            return ContainsCoord(result, 1, 0);
        }

        private static bool CheckClusteredFailRule()
        {
            var matrix = CreateMatrix(5, 5, DieCategory.PassDie, 1);
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    SetDie(matrix, x, y, DieCategory.FailDie, 2);
                }
            }

            var rule = new ClusteredFailInkRule();
            var parameters = rule.GetDefaultParameters();
            var result = rule.Preview(matrix, parameters);

            return ContainsCoord(result, 0, 1);
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
