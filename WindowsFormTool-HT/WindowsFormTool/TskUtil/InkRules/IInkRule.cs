using System;
using System.Collections.Generic;
using DataToExcel;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// INK规则接口
    /// </summary>
    public interface IInkRule
    {
        /// <summary>
        /// 规则ID
        /// </summary>
        string RuleId { get; }

        /// <summary>
        /// 规则名称
        /// </summary>
        string RuleName { get; }

        /// <summary>
        /// 规则描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 是否支持多圈迭代
        /// </summary>
        bool SupportsMultiRing { get; }

        /// <summary>
        /// 获取默认参数
        /// </summary>
        Dictionary<string, object> GetDefaultParameters();

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <returns>验证是否通过</returns>
        bool ValidateParameters(Dictionary<string, object> parameters);

        /// <summary>
        /// 应用规则（预览模式）
        /// </summary>
        /// <param name="matrix">Die矩阵</param>
        /// <param name="parameters">参数</param>
        /// <returns>将被INK的Die坐标列表</returns>
        List<Tuple<int, int>> Preview(DieMatrix matrix, Dictionary<string, object> parameters);

        /// <summary>
        /// 应用规则（实际修改）
        /// </summary>
        /// <param name="matrix">Die矩阵</param>
        /// <param name="parameters">参数</param>
        /// <returns>处理结果</returns>
        InkRuleResult Apply(DieMatrix matrix, Dictionary<string, object> parameters);
    }

    /// <summary>
    /// INK规则应用模式
    /// </summary>
    public enum CrossPatternMode
    {
        /// <summary>
        /// 模式1：纯Fail包围（上下左右四颗均为Fail Die）
        /// </summary>
        PureFailSurround = 1,

        /// <summary>
        /// 模式2：含Mark的Fail包围（1-3颗Mark Die + 其余Fail Die）
        /// </summary>
        MarkFailSurround = 2
    }
}
