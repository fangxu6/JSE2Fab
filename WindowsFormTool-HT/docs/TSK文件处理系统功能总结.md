# TSK文件处理系统功能总结

## 项目概述

这是一个用于处理半导体晶圆测试数据的Windows应用程序，主要功能是读取、解析、转换和合并TSK格式的晶圆测试文件。实现严格遵循TSK_spec_2013.pdf规范。

---

## 核心架构

### 类继承关系

```
IMappingFile (接口)
    ↓
MappingBase (抽象基类)
    ↓
├── Tsk (TSK文件处理)
├── Dat (DAT文件处理)
├── CmdTxt (文本格式输出)
└── 其他格式...
```

---

## 一、核心类详解

### 1. Tsk.cs - TSK文件处理核心类

#### 1.1 主要功能
- **文件读取**：解析二进制TSK文件
- **文件保存**：生成符合规范的TSK文件
- **文件合并**：合并多个TSK文件的测试结果
- **扩展数据支持**：处理多层扩展数据结构

#### 1.2 文件头信息（50+属性）

**基本信息**
- `Operator`: 操作员名称（20字节）
- `Device`: 设备名称（16字节）
- `WaferID`: 晶圆ID（21字节）
- `LotNo`: 批次号（18字节）
- `WaferSize`: 晶圆尺寸（2字节）

**坐标系统**
- `IndexSizeX/Y`: X/Y方向索引尺寸（各4字节）
- `FlatDir`: 平边方向（2字节）
- `XCoordinates`: X坐标增长方向（1=左负，2=右正）
- `YCoordinates`: Y坐标增长方向（1=前正，2=后负）
- `FirstDirX/Y`: 第一个Die的坐标（各4字节）
- `Refpx/Refpy`: 参考点坐标（各2字节）

**测试参数**
- `MachineNo`: 机器编号（2字节）
- `MachineType`: 机器类型（1字节）
- `MapVersion`: 地图版本（1字节，支持2/4/7）
- `ProbingNo`: 探针编号（1字节）
- `Rows`: 矩阵行数（2字节）
- `Cols`: 矩阵列数（2字节）

**时间信息**
- `StartTime`: 测试开始时间（12字节）
- `EndTime`: 测试结束时间（12字节）
- `LoadTime`: 装载时间（12字节）
- `UnloadTime`: 卸载时间（12字节）

**统计数据**
- `TotalDie`: 总Die数（2字节）
- `PassDie`: 通过Die数（2字节）
- `FailDie`: 失败Die数（2字节）

#### 1.3 Die数据结构（每个Die 6字节）

**第一个字（2字节）**
```
Bit 15-14: 测试结果
  00 = None (未测试)
  01 = Pass (通过)
  10 = Fail (失败)
  11 = Fail (失败)
Bit 13: 标记状态 (1=已标记)
Bit 12: 失效标记检查
Bit 11-10: 重测结果
Bit 9-0: X坐标值 (0-511)
```

**第二个字（2字节）**
```
Bit 15-14: Die属性
  00 = Skip (跳过)
  01 = Probing (测试)
  10 = Mark (标记)
  11 = Unknown (未知)
Bit 13: 针标记检查执行
Bit 12: 采样die
Bit 11: Y坐标符号位 (1=负)
Bit 10: X坐标符号位 (1=负)
Bit 9: 虚拟数据标志 (1=Skip, 0=Test)
Bit 8-0: Y坐标值 (0-511)
```

**第三个字（2字节）**
```
Byte 1 Bit 5-0: Site号 (0-63)
Byte 2 Bit 5-0: Bin号 (0-63)
其他位: 拒绝标志、测量完成标志等
```

#### 1.4 坐标计算算法

```csharp
// X坐标计算
die.X = FirstDirX + (XCoordinates == 2 ? index % Rows : -index % Rows)

// Y坐标计算
die.Y = FirstDirY + (YCoordinates == 1 ? index / Rows : -index / Rows)
```

#### 1.5 扩展数据支持

**ExtendHeadFlag（扩展头，172字节）**
- 20字节保留区
- 32字节保留区
- TotalDie统计（4字节）
- PassDie统计（4字节）
- FailDie统计（4字节）
- 44字节保留区
- 64字节保留区

**ExtendFlag（扩展Die数据，每Die 4字节）**
- 支持超过64个Bin的情况
- 根据MapVersion不同，字节序不同：
  - Version 2: 正常字节序
  - Version 4/7: 反转字节序
- 存储扩展的Site和Category信息

**ExtendFlag2（额外扩展）**
- 保存文件末尾的所有剩余字节
- 用于保持文件完整性

#### 1.6 核心方法

**Read()** - 读取TSK文件
```csharp
public override void Read()
```
- 读取文件头信息（约200字节）
- 解析Die矩阵数据（Rows × Cols × 6字节）
- 读取扩展头信息（可选）
- 读取扩展Die数据（可选）
- 读取额外扩展数据（可选）

**Save()** - 保存TSK文件
```csharp
public override void Save()
```
- 写入文件头信息
- 写入Die矩阵数据
- 写入扩展数据（如果存在）

**Merge()** - 合并TSK文件
```csharp
public override IMappingFile Merge(IMappingFile map, string newfile)
```
- 只能合并相同类型的TSK文件
- 将源文件的FailDie状态覆盖到当前文件
- 重新统计PassDie、FailDie、TotalDie
- 保存为新文件

**ReadDie()** - 解析单个Die数据
```csharp
private DieData ReadDie(int index)
```
- 读取6字节二进制数据
- 解析Die属性、Bin号、Site号
- 计算Die坐标
- 返回DieData对象

**WriteDie()** - 写入单个Die数据
```csharp
private void WriteDie(DieData d)
```
- 将DieData对象编码为6字节
- 根据Die属性设置标志位
- 处理坐标符号位

---

### 2. Define.cs - 数据结构定义

#### 2.1 DieCategory枚举 - Die类型
```csharp
public enum DieCategory : short
{
    Unknow = 1,      // 未知
    PassDie = 2,     // 通过
    FailDie = 4,     // 失败
    SkipDie = 8,     // 跳过（虚拟数据=1）
    SkipDie2 = 9,    // 跳过（虚拟数据=0）
    NoneDie = 16,    // 未测试
    MarkDie = 32,    // 标记
    TIRefPass = 64,  // TI参考通过
    TIRefFail = 128  // TI参考失败
}
```

#### 2.2 DieData类 - Die数据
```csharp
public class DieData
{
    public DieCategory Attribute { get; set; }  // Die类型
    public int Bin { get; set; }                // Bin号（-1表示未设置）
    public int X { get; set; }                  // X坐标
    public int Y { get; set; }                  // Y坐标
    public int Site { get; set; }               // 测试站点号
}
```

**主要方法**
- `Clone()`: 克隆Die对象
- `Equals()`: 比较两个Die是否相等
- `operator +`: Die合并运算符（优先级：Mark > None > Fail > Pass）

#### 2.3 DieMatrix类 - Die矩阵

**属性**
- `XMax`: X方向最大值（列数）
- `YMax`: Y方向最大值（行数）
- `Count`: Die总数
- `Items`: Die集合

**索引器**
```csharp
DieData this[int index]           // 线性索引
DieData this[int x, int y]        // 二维索引
```

**核心方法**

**旋转操作**
```csharp
public void DeasilRotate(int degree)  // 顺时针旋转（0/90/180/270度）
private void R90()                     // 旋转90度
private void R180()                    // 旋转180度
private void R270()                    // 旋转270度
```

**偏移操作**
```csharp
public void Offset(OffsetDir dir, int qty)  // 平移矩阵
private void OffsetX(int qty)               // X方向偏移
private void OffsetY(int qty)               // Y方向偏移
```

**扩展/收缩操作**
```csharp
public void Expand(ExpandDir dir, int qty)    // 扩展矩阵
public void Collapse(ExpandDir dir, int qty)  // 收缩矩阵
```

**绘制操作**
```csharp
public void Paint(Graphics g, float width, float height, bool isprint)
```
- 使用预定义颜色绘制Die地图
- PassDie: 绿色 (172, 221, 0)
- FailDie: 红色 (214, 46, 47)
- SkipDie: 紫色 (98, 91, 161)
- MarkDie: 黄色 (255, 222, 0)
- NoneDie: 灰色 (218, 218, 218)

**统计方法**
```csharp
public int DieAttributeStat(DieCategory attr)  // 统计指定类型Die数量
```

**运算符重载**
```csharp
operator +    // 矩阵加法（Die按优先级合并）
operator ==   // 矩阵相等比较
operator !=   // 矩阵不等比较
```

#### 2.4 ConvertConfig类 - 格式转换配置

**功能**
- 从XML配置文件读取转换规则
- 支持字段映射、旋转角度、Notch方向等配置

**属性**
- `From`: 源格式
- `To`: 目标格式
- `Rotate`: 旋转角度
- `NotchAppoint`: Notch指定
- `TrimDir`: 裁剪方向
- `Fields`: 字段映射列表

**配置文件**
```xml
<!-- FieldMapping_TI.xml -->
<mapping from="tsk" to="cmdtxt" rotate="0">
  <field from="Device" to="Device"/>
  <field from="LotNo" to="LotNo"/>
  ...
</mapping>
```

---

### 3. CMDTskToTxt.cs - TSK到TXT转换器

#### 3.1 功能
将TSK格式文件转换为CMD可读的TXT格式

#### 3.2 转换流程
```csharp
public override void Convert(string tskfile, string txtfile)
```

1. **读取源文件**
   - 使用Dat类读取TSK文件
   - 解析所有Die数据

2. **加载转换配置**
   - 从XML读取字段映射规则
   - 获取旋转角度等参数

3. **字段映射**
   - 根据配置映射所有字段
   - 复制Die矩阵数据

4. **统计计算**
   - 重新统计PassDie和FailDie数量
   - 计算良率

5. **旋转处理**
   - 根据配置旋转Die矩阵

6. **保存文件**
   - 调用CmdTxt.Save()生成TXT文件

---

### 4. CmdTxt.cs - CMD文本格式输出

#### 4.1 功能
生成CMD格式的文本文件，包含Die地图和统计信息

#### 4.2 输出格式

**Die地图部分**
```
     01 02 03 04 05 ...
     ++-++-++-++-++-...
001| 01 02    03 04 ...
002| 01       02 03 ...
...
```
- 每个Die显示其Bin号（2位数字）
- PassDie: 显示Bin号（通常为01）
- FailDie: 显示Bin号（02-99）
- NoneDie/SkipDie: 显示空格

**统计信息部分**
```
============ Wafer Information () ===========
  Device: [设备名称]
  Lot NO: [批次号]
  Slot NO: [槽位号]
  Wafer ID: [晶圆ID]
  Operater: [操作员]
  Wafer Size: [尺寸]inch
  Flat Dir: [平边方向]
  Wafer Test Start Time: [开始时间]
  Wafer Test Finish Time: [结束时间]
  Wafer Load Time: [装载时间]
  Wafer Unload Time: [卸载时间]
  Total Test Die: [总数]
  Pass Die: [通过数]
  Fail Die: [失败数]
  Yield: [良率]%
  Rows: [行数]
  Cols: [列数]
```

#### 4.3 Total.txt汇总文件
- 自动生成在同目录下
- 汇总所有转换文件的统计信息
- 累计总Die数、通过数、失败数
- 计算整体良率

#### 4.4 静态统计变量
```csharp
public static int _TotalDie = 0;        // 累计总Die数
public static int _TotalPassDie = 0;    // 累计通过数
public static int _TotalFailDie = 0;    // 累计失败数
public static string _TotalYield = "";  // 累计良率
```

---

### 5. UtilFunction.cs - 工具函数类

#### 5.1 DieCategoryCaption() - Die类型显示
```csharp
public static string DieCategoryCaption(DieCategory cate)
```
- PassDie → "P"
- FailDie → "F"
- MarkDie → "M"
- NoneDie/SkipDie → "."
- 其他 → "?"

#### 5.2 CMDTxtBinText() - CMD格式Bin文本
```csharp
public static string CMDTxtBinText(DieCategory cate)
```
- PassDie → "1"
- FailDie/SkipDie → "3"
- NoneDie/MarkDie → "5"
- 其他 → "?"

#### 5.3 WriteToFile() - 调试日志
```csharp
public static void WriteToFile(string file, string msg)
```
- 将调试信息写入文件
- 默认文件：debug.txt

---

## 二、数据流程

### 1. TSK文件读取流程
```
TSK二进制文件
    ↓
Tsk.Read()
    ↓
解析文件头（200字节）
    ↓
解析Die矩阵（Rows×Cols×6字节）
    ↓
解析扩展头（172字节，可选）
    ↓
解析扩展Die数据（Rows×Cols×4字节，可选）
    ↓
解析额外扩展数据（可选）
    ↓
DieMatrix对象
```

### 2. TSK到TXT转换流程
```
TSK文件
    ↓
Dat.Read() 读取
    ↓
CMDTskToTxt.Convert() 转换
    ↓
字段映射 + 统计计算
    ↓
DieMatrix旋转（可选）
    ↓
CmdTxt.Save() 保存
    ↓
TXT文件 + Total.txt
```

### 3. TSK文件合并流程
```
TSK文件A + TSK文件B
    ↓
Tsk.Merge()
    ↓
遍历所有Die
    ↓
如果B中Die为FailDie
    ↓
覆盖A中对应Die的状态和Bin
    ↓
重新统计PassDie/FailDie/TotalDie
    ↓
保存为新TSK文件
```

---

## 三、关键技术点

### 1. 字节序处理
- TSK文件使用大端序（Big-Endian）
- C#默认使用小端序（Little-Endian）
- 使用Reverse()方法进行字节序转换

```csharp
protected virtual void Reverse(ref byte[] target)
{
    int n1 = 0, n2 = target.Length - 1;
    byte temp;
    while (n1 < n2)
    {
        temp = target[n1];
        target[n1] = target[n2];
        target[n2] = temp;
        n1++;
        n2--;
    }
}
```

### 2. 位操作解析
使用位运算提取Die数据中的各个字段：
```csharp
// 提取测试结果（Bit 6-7）
int dieTestResult = (buffer[0] >> 6) & 0x3;

// 提取Bin号（Bit 0-5）
int binNum = buffer[1] & 0x3f;

// 提取Site号（Bit 0-5）
int siteNum = buffer[0] & 0x3f;
```

### 3. 坐标系统转换
支持多种坐标增长方向：
- X方向：左负（1）或右正（2）
- Y方向：前正（1）或后负（2）

### 4. 版本兼容性
支持多个MapVersion：
- Version 2: 特殊的扩展数据字节序
- Version 4: 扩展数据字节序变化
- Version 7: 扩展数据字节序变化

### 5. 扩展数据处理
三层扩展结构：
1. ExtendHeadFlag: 扩展头信息（172字节）
2. ExtendFlag: 扩展Die数据（每Die 4字节）
3. ExtendFlag2: 额外扩展数据（不定长）

---

## 四、文件格式规范

### TSK文件结构
```
[文件头] (约200字节)
  ├─ 基本信息 (Operator, Device, WaferID等)
  ├─ 坐标系统 (IndexSize, FlatDir, Coordinates等)
  ├─ 测试参数 (MachineNo, MapVersion等)
  ├─ 时间信息 (StartTime, EndTime等)
  └─ 统计数据 (TotalDie, PassDie, FailDie)

[Die数据区] (Rows × Cols × 6字节)
  └─ 每个Die: [Word1][Word2][Word3]

[扩展头] (172字节，可选)
  ├─ 保留区1 (20字节)
  ├─ 保留区2 (32字节)
  ├─ 统计数据 (12字节)
  ├─ 保留区3 (44字节)
  └─ 保留区4 (64字节)

[扩展Die数据] (Rows × Cols × 4字节，可选)
  └─ 每个Die: [ExtWord1][ExtWord2]

[额外扩展数据] (不定长，可选)
```

---

## 五、使用场景

### 1. TSK文件读取
```csharp
Tsk tsk = new Tsk("path/to/file.tsk");
tsk.Read();
Console.WriteLine($"WaferID: {tsk.WaferID}");
Console.WriteLine($"PassDie: {tsk.PassDie}");
Console.WriteLine($"FailDie: {tsk.FailDie}");
```

### 2. TSK文件合并
```csharp
Tsk tsk1 = new Tsk("file1.tsk");
tsk1.Read();

Tsk tsk2 = new Tsk("file2.tsk");
tsk2.Read();

tsk1.Merge(tsk2, "merged.tsk");
```

### 3. TSK转TXT
```csharp
CMDTskToTxt converter = new CMDTskToTxt();
converter.Convert("input.tsk", "output.txt");
```

### 4. Die矩阵操作
```csharp
DieMatrix matrix = tsk.DieMatrix;

// 旋转90度
matrix.DeasilRotate(90);

// 统计FailDie数量
int failCount = matrix.DieAttributeStat(DieCategory.FailDie);

// 访问特定Die
DieData die = matrix[10, 20];
Console.WriteLine($"Bin: {die.Bin}, Site: {die.Site}");
```

---

## 六、注意事项

### 1. 文件编码
- TSK文件为二进制格式
- 字符串使用ASCII编码
- 数值使用大端序

### 2. 坐标系统
- X坐标范围：0-511
- Y坐标范围：0-511
- 支持负坐标（通过符号位）

### 3. Bin号限制
- 基本格式：0-63（6位）
- 扩展格式：0-255（8位）
- PassDie的Bin通常为1
- FailDie的Bin应>1

### 4. 版本兼容
- 不同MapVersion的扩展数据字节序不同
- 读取时需根据版本号判断
- 保存时需保持原版本号

### 5. 错误处理
- 代码中使用Console.WriteLine("error")进行调试
- 检测Bin号异常（PassDie的Bin≠1或FailDie的Bin≤1）
- 扩展数据读取时的容错处理

---

## 七、总结

本系统实现了完整的TSK文件处理功能，包括：

1. **完整的TSK文件解析**：支持文件头、Die数据、多层扩展数据
2. **灵活的格式转换**：TSK → TXT，支持字段映射和旋转
3. **强大的矩阵操作**：旋转、偏移、扩展、收缩等
4. **文件合并功能**：合并多个TSK文件的测试结果
5. **可视化支持**：Die地图绘制功能
6. **版本兼容性**：支持多个MapVersion
7. **统计分析**：自动计算良率和统计信息

代码结构清晰，采用面向对象设计，易于扩展和维护。
