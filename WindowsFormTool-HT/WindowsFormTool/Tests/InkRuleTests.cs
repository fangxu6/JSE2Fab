using System;
using System.Collections.Generic;
using System.Linq;
using DataToExcel;
using WindowsFormTool.TskUtil.InkRules;

namespace DataToExcel.Tests
{
    /// <summary>
    /// INK规则单元测试
    /// </summary>
    public class InkRuleTests
    {
        private int _passedTests = 0;
        private int _failedTests = 0;

        /// <summary>
        /// 通过的测试数量
        /// </summary>
        public int PassedTests => _passedTests;

        /// <summary>
        /// 失败的测试数量
        /// </summary>
        public int FailedTests => _failedTests;

        public void RunAllTests()
        {
            Console.WriteLine("=== 十字围点规则测试 ===");
            RunTest("模式1: 四邻域均为Fail - 应该INK", Test_CrossPattern_Mode1_SurroundedByFail);
            RunTest("模式1: 边缘Die不应INK", Test_CrossPattern_Mode1_EdgeDie);
            RunTest("模式1: 无匹配情况", Test_CrossPattern_Mode1_NoMatch);
            RunTest("模式1: 混合场景", Test_CrossPattern_Mode1_Mixed);
            RunTest("模式2: 含1颗Mark - 应该INK", Test_CrossPattern_Mode2_OneMark);
            RunTest("模式2: 含3颗Mark - 应该INK", Test_CrossPattern_Mode2_ThreeMarks);
            RunTest("模式2: 4颗Mark - 不应该INK", Test_CrossPattern_Mode2_FourMarks);
            RunTest("模式2: 无Mark - 不应该INK", Test_CrossPattern_Mode2_NoMark);

            Console.WriteLine("\n=== 九宫格规则测试 ===");
            RunTest("九宫格: 1圈 - 检测到Fail", Test_NineGrid_OneRing);
            RunTest("九宫格: 2圈 - 迭代处理", Test_NineGrid_TwoRings);
            RunTest("九宫格: 3圈 - 扩展处理", Test_NineGrid_ThreeRings);
            RunTest("九宫格: 无Fail - 不应INK", Test_NineGrid_NoFail);
            RunTest("九宫格: 边缘处理", Test_NineGrid_EdgeHandling);

            Console.WriteLine("\n=== InkRuleManager测试 ===");
            RunTest("Manager: 获取所有规则", Test_InkRuleManager_GetAllRules);
            RunTest("Manager: 参数验证", Test_InkRuleManager_ValidateParameters);

            Console.WriteLine("\n=== DieMatrix INK方法测试 ===");
            RunTest("DieMatrix: ApplyInkRule", Test_DieMatrix_ApplyInkRule);
            RunTest("DieMatrix: PreviewInkResult", Test_DieMatrix_PreviewInkResult);
            RunTest("DieMatrix: GetBinDistribution", Test_DieMatrix_GetBinDistribution);
        }

        private void RunTest(string testName, Func<bool> testFunc)
        {
            try
            {
                if (testFunc())
                {
                    Console.WriteLine($"  ✓ {testName}");
                    _passedTests++;
                }
                else
                {
                    Console.WriteLine($"  ✗ {testName}");
                    _failedTests++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ {testName}: {ex.Message}");
                _failedTests++;
            }
        }

        #region 十字围点规则测试

        private bool Test_CrossPattern_Mode1_SurroundedByFail()
        {
            // 创建3x3矩阵，中心Die被四颗Fail包围
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 设置四邻域为Fail (Bin=2)
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 1, 2, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 1 &&
                   matrix[1, 1].Bin == 63;
        }

        private bool Test_CrossPattern_Mode1_EdgeDie()
        {
            // 创建3x3矩阵，边缘Die不应被INK
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 0, 0, 1); // 左上角Pass Die

            // 设置右侧和下方为Fail
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 0, 1, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 0 &&
                   matrix[0, 0].Bin == 1;
        }

        private bool Test_CrossPattern_Mode1_NoMatch()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 只有两颗Fail，不满足四邻域条件
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 0;
        }

        private bool Test_CrossPattern_Mode1_Mixed()
        {
            // 创建5x5矩阵，多个场景
            var matrix = CreateMatrix(5, 5);

            // 场景1: (2,2) 被四颗Fail包围
            SetDieBin(matrix, 2, 2, 1);
            SetDieBin(matrix, 1, 2, 2);
            SetDieBin(matrix, 3, 2, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 2, 3, 2);

            // 场景2: (0,0) 边缘Die不应被INK
            SetDieBin(matrix, 0, 0, 1);
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 0, 1, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 1 &&
                   matrix[2, 2].Bin == 63 &&
                   matrix[0, 0].Bin == 1;
        }

        private bool Test_CrossPattern_Mode2_OneMark()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 三颗Fail，一颗Mark
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 1, 0, 2);
            SetDieAttribute(matrix, 1, 2, DieCategory.MarkDie);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 2 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 1 &&
                   matrix[1, 1].Bin == 63;
        }

        private bool Test_CrossPattern_Mode2_ThreeMarks()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 一颗Fail，三颗Mark
            SetDieBin(matrix, 0, 1, 2);
            SetDieAttribute(matrix, 2, 1, DieCategory.MarkDie);
            SetDieAttribute(matrix, 1, 0, DieCategory.MarkDie);
            SetDieAttribute(matrix, 1, 2, DieCategory.MarkDie);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 2 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 1 &&
                   matrix[1, 1].Bin == 63;
        }

        private bool Test_CrossPattern_Mode2_FourMarks()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 四颗都是Mark
            SetDieAttribute(matrix, 0, 1, DieCategory.MarkDie);
            SetDieAttribute(matrix, 2, 1, DieCategory.MarkDie);
            SetDieAttribute(matrix, 1, 0, DieCategory.MarkDie);
            SetDieAttribute(matrix, 1, 2, DieCategory.MarkDie);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 2 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 0;
        }

        private bool Test_CrossPattern_Mode2_NoMark()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1); // 中心Pass Die

            // 四颗都是Fail（无Mark）- 模式2不应匹配
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 1, 2, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 2 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 0;
        }

        #endregion

        #region 九宫格规则测试

        private bool Test_NineGrid_OneRing()
        {
            var matrix = CreateMatrix(5, 5);
            SetDieBin(matrix, 2, 2, 1); // 中心Pass Die
            SetDieBin(matrix, 1, 1, 2); // 3x3区域内的Fail

            var rule = new NineGridInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount > 0;
        }

        private bool Test_NineGrid_TwoRings()
        {
            var matrix = CreateMatrix(7, 7);
            SetDieBin(matrix, 3, 3, 1); // 中心Pass Die
            SetDieBin(matrix, 2, 2, 2); // 内部Fail

            var rule = new NineGridInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 2 }
            };

            var result1Ring = rule.Apply(matrix, new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 1 }
            });

            var matrix2 = CreateMatrix(7, 7);
            SetDieBin(matrix2, 3, 3, 1);
            SetDieBin(matrix2, 2, 2, 2);
            var result2Rings = new NineGridInkRule().Apply(matrix2, parameters);

            return result2Rings.Success &&
                   result2Rings.TotalInkedCount >= result1Ring.TotalInkedCount;
        }

        private bool Test_NineGrid_ThreeRings()
        {
            var matrix = CreateMatrix(9, 9);
            SetDieBin(matrix, 4, 4, 1); // 中心Pass Die
            SetDieBin(matrix, 3, 3, 2); // 内部Fail

            var rule = new NineGridInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 3 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount > 0;
        }

        private bool Test_NineGrid_NoFail()
        {
            var matrix = CreateMatrix(5, 5);
            // 所有Die都是Pass
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    SetDieBin(matrix, x, y, 1);
                }
            }

            var rule = new NineGridInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 1 }
            };

            var result = rule.Apply(matrix, parameters);
            return result.Success && result.TotalInkedCount == 0;
        }

        private bool Test_NineGrid_EdgeHandling()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 0, 0, 1); // 左上角Pass Die
            SetDieBin(matrix, 1, 0, 2); // 相邻Fail

            var rule = new NineGridInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "rings", 1 }
            };

            // 应该不崩溃，边缘Die使用实际存在的邻域
            var result = rule.Apply(matrix, parameters);
            return result.Success;
        }

        #endregion

        #region InkRuleManager测试

        private bool Test_InkRuleManager_GetAllRules()
        {
            var manager = InkRuleManager.Instance;
            var rules = manager.GetAllRules();

            return rules.Count >= 2 &&
                   rules.Any(r => r.RuleId == CrossPatternInkRule.RULE_ID) &&
                   rules.Any(r => r.RuleId == NineGridInkRule.RULE_ID);
        }

        private bool Test_InkRuleManager_ValidateParameters()
        {
            var manager = InkRuleManager.Instance;

            // 有效参数
            var validParams = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };
            var valid = manager.ValidateParameters(CrossPatternInkRule.RULE_ID, validParams);

            // 无效参数
            var invalidParams = new Dictionary<string, object>
            {
                { "targetBinNo", 0 }, // 无效的Bin号
                { "mode", 1 }
            };
            var invalid = manager.ValidateParameters(CrossPatternInkRule.RULE_ID, invalidParams);

            return valid && !invalid;
        }

        #endregion

        #region DieMatrix INK方法测试

        private bool Test_DieMatrix_ApplyInkRule()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1);
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 1, 2, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var result = matrix.ApplyInkRule(rule, parameters);
            return result.Success && result.TotalInkedCount == 1;
        }

        private bool Test_DieMatrix_PreviewInkResult()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 1, 1, 1);
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 2, 1, 2);
            SetDieBin(matrix, 1, 0, 2);
            SetDieBin(matrix, 1, 2, 2);

            var rule = new CrossPatternInkRule();
            var parameters = new Dictionary<string, object>
            {
                { "targetBinNo", 63 },
                { "mode", 1 }
            };

            var preview = matrix.PreviewInkResult(rule, parameters);
            return preview.Count == 1 && preview[0].Item1 == 1 && preview[0].Item2 == 1;
        }

        private bool Test_DieMatrix_GetBinDistribution()
        {
            var matrix = CreateMatrix(3, 3);
            SetDieBin(matrix, 0, 0, 1);
            SetDieBin(matrix, 1, 0, 1);
            SetDieBin(matrix, 2, 0, 2);
            SetDieBin(matrix, 0, 1, 2);
            SetDieBin(matrix, 1, 1, 2);
            SetDieBin(matrix, 2, 1, 3);
            SetDieBin(matrix, 0, 2, 3);
            SetDieBin(matrix, 1, 2, 3);
            SetDieBin(matrix, 2, 2, 3);

            var distribution = matrix.GetBinDistribution();

            return distribution.Count == 3 &&
                   distribution[1] == 2 &&
                   distribution[2] == 3 &&
                   distribution[3] == 4;
        }

        #endregion

        #region 辅助方法

        private DieMatrix CreateMatrix(int xMax, int yMax)
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

        private void SetDieBin(DieMatrix matrix, int x, int y, int bin)
        {
            var die = matrix[x, y];
            die.Bin = bin;
            if (bin == 1)
                die.Attribute = DieCategory.PassDie;
            else
                die.Attribute = DieCategory.FailDie;
        }

        private void SetDieAttribute(DieMatrix matrix, int x, int y, DieCategory attribute)
        {
            var die = matrix[x, y];
            die.Attribute = attribute;
        }

        #endregion
    }
}
