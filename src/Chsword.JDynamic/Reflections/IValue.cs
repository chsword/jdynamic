using System;

namespace Chsword.Reflections
{
    /// <summary>
    /// ��Ա���ٻ�ֵ��ֵ�ӿ�
    /// </summary>
    public interface IValue
    {
        /// <summary>
        /// ��ȡ��Ա���ٻ�ֵί�С�
        /// </summary>
        Func<object, object> GetValue { get; }
    }
}