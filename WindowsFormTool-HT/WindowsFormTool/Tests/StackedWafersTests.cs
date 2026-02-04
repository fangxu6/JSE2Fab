using System;
using System.Collections.Generic;
using DataToExcel;
using WindowsFormTool.TskUtil.StackedWafers;

namespace DataToExcel.Tests
{
    public class StackedWafersTests
    {
        private int _passedTests = 0;
        private int _failedTests = 0;
        private int _skippedTests = 0;

        public int PassedTests => _passedTests;

        public int FailedTests => _failedTests;

        public int SkippedTests => _skippedTests;

        public void RunAllTests()
        {
            Console.WriteLine("=== Stacked Wafers Tests ===");
            RunTest("Stacked: floor threshold", Test_FloorThreshold);
            RunTest("Stacked: mismatched coordinates", Test_MismatchedCoordinates);
            RunTest("Stacked: null die mismatch", Test_NullDieMismatch);
            RunTest("Stacked: apply to all wafers", Test_ApplyToAllWafers);
            RunTest("Stacked: threshold too low", Test_ThresholdTooLow);
            RunTest("Stacked: invalid thresholds", Test_InvalidThresholds);
            RunTest("Stacked: apply target bin", Test_ApplyTargetBin);
            RunTest("Stacked: skip marked die", Test_ApplySkipsMarkedDie);
            RunTest("Stacked: skip none die", Test_ApplySkipsNoneDie);
        }

        private void RunTest(string testName, Func<bool> testFunc)
        {
            try
            {
                if (testFunc())
                {
                    Console.WriteLine("  PASS " + testName);
                    _passedTests++;
                }
                else
                {
                    Console.WriteLine("  FAIL " + testName);
                    _failedTests++;
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("  SKIP " + testName + " (not implemented)");
                _skippedTests++;
            }
            catch (Exception ex)
            {
                Console.WriteLine("  FAIL " + testName + ": " + ex.Message);
                _failedTests++;
            }
        }

        private bool Test_FloorThreshold()
        {
            return StackedWafersCalculator.RequiredFailCount(0.75, 25) == 18;
        }

        private bool Test_MismatchedCoordinates()
        {
            var a = TestMatrix(2, 2);
            var b = TestMatrix(2, 2);
            b[1, 1].X = 9;
            return !StackedWafersCalculator.TryValidateSameShape(new[] { a, b }, out _);
        }

        private bool Test_NullDieMismatch()
        {
            var a = TestMatrix(2, 2);
            var dies = new List<DieData>();
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    dies.Add(x == 1 && y == 0 ? null : new DieData
                    {
                        X = x,
                        Y = y,
                        Bin = 1,
                        Attribute = DieCategory.PassDie,
                        Site = 0
                    });
                }
            }

            var b = new DieMatrix(dies, 2, 2);
            return !StackedWafersCalculator.TryValidateSameShape(new[] { a, b }, out _);
        }

        private bool Test_ApplyToAllWafers()
        {
            var wafers = BuildTwoWaferLot();
            var coords = StackedWafersCalculator.ComputeStackedBadCoordinates(wafers, 0.5);
            return coords.Contains(new Tuple<int, int>(1, 1));
        }

        private bool Test_ThresholdTooLow()
        {
            var wafers = BuildTwoWaferLot();
            try
            {
                StackedWafersCalculator.ComputeStackedBadCoordinates(wafers, 0.1);
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
        }

        private bool Test_InvalidThresholds()
        {
            return ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(0, 2)) &&
                   ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(-0.1, 2)) &&
                   ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(1.1, 2)) &&
                   ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(double.NaN, 2)) &&
                   ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(double.PositiveInfinity, 2)) &&
                   ExpectThrows(() => StackedWafersCalculator.RequiredFailCount(0.5, 0));
        }

        private bool Test_ApplyTargetBin()
        {
            var matrix = TestMatrix(2, 2);
            var coords = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(1, 1)
            };

            var applied = StackedWafersCalculator.ApplyStackedBadCoordinates(matrix, coords, 63);
            return applied == 1 &&
                   matrix[1, 1].Bin == 63 &&
                   matrix[1, 1].Attribute == DieCategory.FailDie;
        }

        private bool Test_ApplySkipsMarkedDie()
        {
            var matrix = TestMatrix(2, 2);
            matrix[0, 0].Attribute = DieCategory.MarkDie;
            matrix[0, 0].Bin = 10;

            var coords = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(0, 0)
            };

            var applied = StackedWafersCalculator.ApplyStackedBadCoordinates(matrix, coords, 63);
            return applied == 0 &&
                   matrix[0, 0].Bin == 10 &&
                   matrix[0, 0].Attribute == DieCategory.MarkDie;
        }

        private bool Test_ApplySkipsNoneDie()
        {
            var matrix = TestMatrix(2, 2);
            matrix[1, 0].Attribute = DieCategory.NoneDie;
            matrix[1, 0].Bin = 0;

            var coords = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(1, 0)
            };

            var applied = StackedWafersCalculator.ApplyStackedBadCoordinates(matrix, coords, 63);
            return applied == 0 &&
                   matrix[1, 0].Bin == 0 &&
                   matrix[1, 0].Attribute == DieCategory.NoneDie;
        }

        private DieMatrix TestMatrix(int xMax, int yMax)
        {
            var dies = new List<DieData>();
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    var die = new DieData
                    {
                        X = x,
                        Y = y,
                        Bin = 1,
                        Attribute = DieCategory.PassDie,
                        Site = 0
                    };
                    dies.Add(die);
                }
            }

            return new DieMatrix(dies, xMax, yMax);
        }

        private List<DieMatrix> BuildTwoWaferLot()
        {
            var first = TestMatrix(2, 2);
            var second = TestMatrix(2, 2);
            first[1, 1].Attribute = DieCategory.FailDie;
            return new List<DieMatrix> { first, second };
        }

        private bool ExpectThrows(Action action)
        {
            try
            {
                action();
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
        }
    }
}
