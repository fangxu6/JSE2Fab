using System;
using System.Collections.Generic;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// INK规则处理结果
    /// </summary>
    public class InkRuleResult
    {
        /// <summary>
        /// 处理的规则ID
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 处理的规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 被INK的Die坐标列表
        /// </summary>
        public List<Tuple<int, int>> InkedDies { get; set; }

        /// <summary>
        /// 总共INK的Die数量
        /// </summary>
        public int TotalInkedCount { get; set; }

        /// <summary>
        /// 按原Bin号统计的INK数量
        /// </summary>
        public Dictionary<int, int> InkedCountByBin { get; set; }

        /// <summary>
        /// 按圈数统计的INK数量（多圈场景）
        /// </summary>
        public Dictionary<int, int> InkedCountByRing { get; set; }

        /// <summary>
        /// 处理耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 使用的参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessedTime { get; set; }

        public InkRuleResult()
        {
            InkedDies = new List<Tuple<int, int>>();
            InkedCountByBin = new Dictionary<int, int>();
            InkedCountByRing = new Dictionary<int, int>();
            Success = true;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取统计摘要文本
        /// </summary>
        public string GetSummaryText()
        {
            if (!Success)
            {
                return $"处理失败：{ErrorMessage}";
            }

            var summary = $"共INK {TotalInkedCount} 颗Die";
            if (InkedCountByBin.Count > 0)
            {
                var binStats = new List<string>();
                foreach (var kvp in InkedCountByBin)
                {
                    binStats.Add($"Bin {kvp.Key}→{Parameters["targetBinNo"]}: {kvp.Value}颗");
                }
                summary += $"（{string.Join("，", binStats)}）";
            }

            if (InkedCountByRing.Count > 0)
            {
                var ringStats = new List<string>();
                foreach (var kvp in InkedCountByRing)
                {
                    ringStats.Add($"第{kvp.Key}圈: {kvp.Value}颗");
                }
                summary += $"\n圈数统计：{string.Join("，", ringStats)}";
            }

            return summary;
        }
    }
}
