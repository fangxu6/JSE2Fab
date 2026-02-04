using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.StackedWafers
{
    public static class StackedWafersCalculator
    {
        public static int RequiredFailCount(double threshold, int waferCount)
        {
            if (waferCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waferCount), "waferCount must be positive.");
            }

            if (double.IsNaN(threshold) || double.IsInfinity(threshold) || threshold <= 0 || threshold > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "threshold must be in (0, 1].");
            }

            var required = Math.Floor((decimal)threshold * waferCount);
            if (required < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "threshold is too low for the wafer count.");
            }

            return (int)required;
        }

        public static List<Tuple<int, int>> ComputeStackedBadCoordinates(List<DieMatrix> matrices, double threshold)
        {
            if (!TryValidateSameShape(matrices, out var error))
            {
                throw new InvalidOperationException(error ?? "matrices are not compatible.");
            }

            var waferCount = matrices.Count;
            var requiredFailCount = RequiredFailCount(threshold, waferCount);
            var reference = matrices[0];
            var result = new List<Tuple<int, int>>();

            for (int y = 0; y < reference.YMax; y++)
            {
                for (int x = 0; x < reference.XMax; x++)
                {
                    int failCount = 0;
                    for (int i = 0; i < waferCount; i++)
                    {
                        var die = matrices[i][x, y];
                        if (die.Attribute == DieCategory.FailDie)
                        {
                            failCount++;
                        }
                    }

                    if (failCount >= requiredFailCount)
                    {
                        var die = reference[x, y];
                        result.Add(new Tuple<int, int>(die.X, die.Y));
                    }
                }
            }

            return result;
        }

        public static int ApplyStackedBadCoordinates(DieMatrix matrix, List<Tuple<int, int>> coordinates, int targetBinNo)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            if (targetBinNo < 1 || targetBinNo > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(targetBinNo), "targetBinNo must be in [1, 255].");
            }

            var coordinateMap = BuildCoordinateIndex(matrix);
            int applied = 0;

            foreach (var coord in coordinates)
            {
                if (!coordinateMap.TryGetValue(coord, out var index))
                {
                    continue;
                }

                var die = matrix[index.Item1, index.Item2];
                if (die.Attribute == DieCategory.PassDie)
                {
                    die.Bin = targetBinNo;
                    die.Attribute = DieCategory.FailDie;
                    applied++;
                }
            }

            return applied;
        }

        public static bool TryValidateSameShape(IEnumerable<DieMatrix> matrices, out string error)
        {
            if (matrices == null)
            {
                error = "matrices cannot be null.";
                return false;
            }

            error = null;
            DieMatrix reference = null;
            int index = 0;

            foreach (var matrix in matrices)
            {
                if (matrix == null)
                {
                    error = "Matrix at index " + index + " is null.";
                    return false;
                }

                if (reference == null)
                {
                    reference = matrix;
                    index++;
                    continue;
                }

                if (matrix.XMax != reference.XMax || matrix.YMax != reference.YMax)
                {
                    error = "Matrix shape mismatch at index " + index + ".";
                    return false;
                }

                if (!TryValidateSameCoordinates(reference, matrix, out error))
                {
                    error = "Matrix coordinate mismatch at index " + index + ": " + error;
                    return false;
                }

                index++;
            }

            if (reference == null)
            {
                error = "matrices must contain at least one item.";
                return false;
            }

            return true;
        }

        private static bool TryValidateSameCoordinates(DieMatrix left, DieMatrix right, out string error)
        {
            for (int y = 0; y < left.YMax; y++)
            {
                for (int x = 0; x < left.XMax; x++)
                {
                    var leftDie = left[x, y];
                    var rightDie = right[x, y];
                    if (leftDie == null || rightDie == null)
                    {
                        error = "null die at (" + x + "," + y + ")";
                        return false;
                    }
                    if (leftDie.X != rightDie.X || leftDie.Y != rightDie.Y)
                    {
                        error = "coordinate mismatch at (" + x + "," + y + ")";
                        return false;
                    }
                }
            }

            error = null;
            return true;
        }

        private static Dictionary<Tuple<int, int>, Tuple<int, int>> BuildCoordinateIndex(DieMatrix matrix)
        {
            var map = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

            for (int y = 0; y < matrix.YMax; y++)
            {
                for (int x = 0; x < matrix.XMax; x++)
                {
                    var die = matrix[x, y];
                    if (die == null)
                    {
                        continue;
                    }

                    var key = new Tuple<int, int>(die.X, die.Y);
                    if (!map.ContainsKey(key))
                    {
                        map[key] = new Tuple<int, int>(x, y);
                    }
                }
            }

            return map;
        }

        private static bool IsProtectedAttribute(DieCategory attribute)
        {
            return attribute == DieCategory.MarkDie ||
                   attribute == DieCategory.SkipDie ||
                   attribute == DieCategory.SkipDie2 ||
                   attribute == DieCategory.NoneDie;
        }
    }
}
