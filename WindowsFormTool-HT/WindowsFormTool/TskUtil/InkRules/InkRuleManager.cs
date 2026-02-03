using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormTool.TskUtil.InkRules
{
    /// <summary>
    /// INK规则管理器
    /// 负责管理所有可用的INK规则
    /// </summary>
    public class InkRuleManager
    {
        private static InkRuleManager _instance;
        private static readonly object _lock = new object();

        private Dictionary<string, IInkRule> _rules;

        public static InkRuleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InkRuleManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private InkRuleManager()
        {
            _rules = new Dictionary<string, IInkRule>();
            RegisterBuiltInRules();
        }

        /// <summary>
        /// 注册内置规则
        /// </summary>
        private void RegisterBuiltInRules()
        {
            // 注册十字围点规则
            Register(new CrossPatternInkRule());

            // 注册九宫格规则
            Register(new NineGridInkRule());

            // 注册被Fail包围的Pass规则
            Register(new EnclosedPassInkRule());

            // 注册线状Fail扩散规则
            Register(new LineBlobInkRule());

            // 注册团簇Fail扩散规则
            Register(new ClusteredFailInkRule());

            // 注册GDBC九宫格阈值规则
            Register(new GdbcNineGridThresholdInkRule());
        }

        /// <summary>
        /// 注册规则
        /// </summary>
        /// <param name="rule">INK规则实例</param>
        public void Register(IInkRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            if (_rules.ContainsKey(rule.RuleId))
            {
                throw new ArgumentException($"规则已存在：{rule.RuleId}");
            }

            _rules[rule.RuleId] = rule;
        }

        /// <summary>
        /// 注销规则
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <returns>是否成功注销</returns>
        public bool Unregister(string ruleId)
        {
            return _rules.Remove(ruleId);
        }

        /// <summary>
        /// 获取指定规则
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <returns>规则实例，不存在返回null</returns>
        public IInkRule GetRule(string ruleId)
        {
            _rules.TryGetValue(ruleId, out var rule);
            return rule;
        }

        /// <summary>
        /// 获取所有可用规则
        /// </summary>
        /// <returns>规则列表</returns>
        public List<IInkRule> GetAllRules()
        {
            return _rules.Values.ToList();
        }

        /// <summary>
        /// 获取所有规则ID
        /// </summary>
        public List<string> GetAllRuleIds()
        {
            return _rules.Keys.ToList();
        }

        /// <summary>
        /// 获取所有规则名称
        /// </summary>
        public List<string> GetAllRuleNames()
        {
            return _rules.Values.Select(r => r.RuleName).ToList();
        }

        /// <summary>
        /// 根据名称获取规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <returns>规则实例</returns>
        public IInkRule GetRuleByName(string ruleName)
        {
            return _rules.Values.FirstOrDefault(r => r.RuleName == ruleName);
        }

        /// <summary>
        /// 获取规则数量
        /// </summary>
        public int Count => _rules.Count;

        /// <summary>
        /// 验证规则参数
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <param name="parameters">参数</param>
        /// <returns>验证结果</returns>
        public bool ValidateParameters(string ruleId, Dictionary<string, object> parameters)
        {
            var rule = GetRule(ruleId);
            if (rule == null)
                return false;

            return rule.ValidateParameters(parameters);
        }

        /// <summary>
        /// 获取规则的默认参数
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <returns>默认参数字典</returns>
        public Dictionary<string, object> GetDefaultParameters(string ruleId)
        {
            var rule = GetRule(ruleId);
            if (rule == null)
                return null;

            return rule.GetDefaultParameters();
        }

        /// <summary>
        /// 清除所有规则
        /// </summary>
        public void Clear()
        {
            _rules.Clear();
        }

        /// <summary>
        /// 重置为内置规则
        /// </summary>
        public void Reset()
        {
            _rules.Clear();
            RegisterBuiltInRules();
        }
    }
}
