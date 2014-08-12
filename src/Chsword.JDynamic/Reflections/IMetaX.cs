using System.Reflection;

namespace Chsword.Reflections
{
    /// <summary>
    /// 泛型快速成员访问接口
    /// </summary>
    /// <typeparam name="T">从 <paramref name="System.Reflection.MemberInfo"/> 类继承的类型</typeparam>
    public interface IMetaX<out T> where T : MemberInfo
    {
        /// <summary>
        /// 获取成员元数据。
        /// </summary>
        T Meta { get; }
        /// <summary>
        /// 获取成员元数据名称。
        /// </summary>
        string Name { get; }
    }
}
