# TSK Ink 功能技术总结

## 概述

图谱Ink功能用于检测并修改被Fail Die包围的Pass Die，将它们标记为Fail以提高良率分析的准确性。

## 文件结构

```
WindowsFormTool-HT/WindowsFormTool/
├── TskUtil/InkRules/
│   ├── IInkRule.cs          # INK规则接口
│   ├── InkRuleResult.cs     # 处理结果类
│   ├── CrossPatternInkRule.cs  # 十字围点规则
│   ├── NineGridInkRule.cs   # 九宫格规则
│   └── InkRuleManager.cs    # 规则管理器
├── Forms/
│   └── InkRuleDialog.cs     # INK规则选择对话框
└── Util/
    └── Define.cs            # DieMatrix类添加INK方法
```

## 核心组件

### 1. IInkRule 接口

```csharp
public interface IInkRule
{
    string RuleId { get; }
    string RuleName { get; }
    string Description { get; }
    bool SupportsMultiRing { get; }

    Dictionary<string, object> GetDefaultParameters();
    bool ValidateParameters(Dictionary<string, object> parameters);
    List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters);
    InkRuleResult Apply(DieMatrix matrix, Dictionary<string, object> parameters);
}
```

### 2. InkRuleResult 类

处理结果的返回对象，包含：
- `RuleId` / `RuleName`: 规则标识
- `Success`: 是否成功
- `ErrorMessage`: 错误信息
- `InkedDies`: 被INK的Die坐标列表
- `TotalInkedCount`: 总数
- `InkedCountByBin`: 按原Bin统计
- `InkedCountByRing`: 按圈数统计（多圈场景）
- `ElapsedMilliseconds`: 处理耗时
- `GetSummaryText()`: 获取统计摘要

### 3. CrossPatternInkRule 十字围点规则

**模式1：纯Fail包围**
- 条件：Pass Die的上下左右四颗均为Fail Die（Bin ≠ 1）
- 结果：该Pass Die → Fail Die（指定Bin）

**模式2：含Mark的Fail包围**
- 条件：Pass Die的上下左右有1-3颗Mark Die，其余为Fail Die
- 结果：该Pass Die → Fail Die（指定Bin）

**参数：**
- `targetBinNo`: 目标Bin号（1-255）
- `mode`: 模式选择（1或2）

### 4. NineGridInkRule 九宫格规则

- 检测3x3区域内存在Fail Die时，将周围Pass Die标记为Fail
- 支持1-3圈迭代处理

**参数：**
- `targetBinNo`: 目标Bin号（1-255）
- `rings`: 圈数（1-3）

### 5. InkRuleManager 规则管理器

单例模式，管理所有可用规则：
- `Register(IInkRule rule)`: 注册规则
- `GetRule(string ruleId)`: 获取规则
- `GetAllRules()`: 获取所有规则
- `ValidateParameters()`: 验证参数

### 6. DieMatrix INK方法

```csharp
public InkRuleResult ApplyInkRule(IInkRule rule, Dictionary<string, object> parameters)
public List<Tuple<int, int>> PreviewInkResult(IInkRule rule, Dictionary<string, object> parameters)
public List<DieData> GetInkCandidates(IInkRule rule, Dictionary<string, object> parameters)
public Dictionary<int, int> GetBinDistribution()
public int CountByAttribute(DieCategory attribute)
```

## Die数据结构

```csharp
public class DieData
{
    public DieCategory Attribute { get; set; }  // Die属性
    public int Bin { get; set; }                 // Bin号
    public int X { get; set; }                   // X坐标
    public int Y { get; set; }                   // Y坐标
    public int Site { get; set; }                // Site号
}
```

**DieCategory枚举：**
- `PassDie` (2): 良品
- `FailDie` (4): 不良品
- `MarkDie` (32): 标记Die
- `SkipDie` (8): 跳过Die
- `NoneDie` (16): 空Die
- `Unknow` (1): 未知

## 边界处理

- 边缘Die（缺少邻域）不触发INK规则
- 九宫格规则对于边界区域使用实际存在的邻域Die进行判断

## UI集成

### Form1.cs INK功能入口

```csharp
// 下拉框选择"INK规则"后
case 1:
    button6.Click += loadFirstFile_Click_INK;  // 选择TSK文件
    button2.Click += button2_Click_INK;        // 执行INK
    break;
```

### InkRuleDialog

规则选择对话框，包含：
- 规则下拉选择
- 目标Bin号输入
- 十字围点模式选择（模式1/模式2）
- 九宫格圈数选择（1-3圈）
- 预览功能
- 应用/取消按钮

## 使用流程

1. 选择"INK规则"功能
2. 点击"选择TSK文件"加载TSK文件
3. 点击"执行INK"打开规则对话框
4. 选择规则类型和参数
5. 点击"预览"查看将被INK的Die
6. 点击"应用"执行INK
7. 确认保存修改后的TSK文件

## 处理流程

```
加载TSK文件
    ↓
打开INK规则对话框
    ↓
选择规则、配置参数
    ↓
预览（可选）
    ↓
应用INK规则
    ↓
更新Die状态（Bin、Attribute）
    ↓
统计处理结果
    ↓
询问是否保存
    ↓
保存TSK文件
```

## 注意事项

1. **Bin号范围**: 目标Bin号必须在1-255之间
2. **边缘Die**: 位于矩阵边缘的Die不会触发任何INK规则
3. **Mark Die**: Mark Die视为Fail Die参与包围判断
4. **多圈迭代**: 九宫格规则支持1-3圈迭代，每圈基于上一圈结果
5. **不可逆**: INK操作直接修改Die状态，保存后不可撤销

---

**版本**: 1.0.0
**更新日期**: 2026-01-28
