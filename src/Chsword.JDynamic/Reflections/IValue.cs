using System;

namespace Chsword.Reflections
{
    /// <summary>
    /// 成员快速获值设值接口
    /// </summary>
    public interface IValue
    {
        /// <summary>
        /// 获取成员快速获值委托。
        /// </summary>
        Func<object, object> GetValue { get; }
    }
}