using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Chsword.Reflections
{
    /// <summary>
    /// 快速类型信息
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerDisplay("[{Meta.FullName}]")]
    public class TypeX : MetaXBase<Type>
    {
        #region Cache Data
        private static ConcurrentDictionary<Type, TypeX> cache = new ConcurrentDictionary<Type, TypeX>();

        /// <summary>
        /// 获得一个 <paramref name="TypeX"/> 类的实例对象。
        /// </summary>
        /// <param name="type">要获得的元数据类型。</param>
        /// <returns>一个 <paramref name="TypeX"/> 类的实例对象。</returns>
        public static TypeX GetTypeX(Type type)
        {
            TypeX result = null;
            if (!cache.TryGetValue(type, out result))
            {
                result = new TypeX(type);
                cache.TryAdd(type, result);
            }
            return result;
        }

        #region Implicit Convert
        /// <summary>
        /// Performs an implicit conversion from <see cref="Lenic.Reflections.TypeX"/> to <see cref="System.Type"/>.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Type(TypeX obj)
        {
            return obj.Meta;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Type"/> to <see cref="Lenic.Reflections.TypeX"/>.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TypeX(Type obj)
        {
            return GetTypeX(obj);
        }
        #endregion
        #endregion

        #region Constructor
        private TypeX(Type type)
        {
            Meta = type;

            InitProperties();
        }

        private static BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        #endregion

        #region Property Data
        private IDictionary<string, PropertyInfoX[]> propertyMetaDic = null;
        /// <summary>
        /// 获得所有属性列表【子类 => 祖先】顺序
        /// </summary>
        public IEnumerable<PropertyInfoX> FullProperties
        {
            get
            {
                foreach (var item in propertyMetaDic.Values)
                {
                    foreach (var inner in item)
                    {
                        yield return inner;
                    }
                }
            }
        }

        /// <summary>
        /// 获得所有属性列表。如果继承链上包含相同名称的属性，则只取最子类的属性。
        /// </summary>
        public IEnumerable<PropertyInfoX> Properties
        {
            get
            {
                foreach (var item in propertyMetaDic.Values)
                {
                    yield return item[0];
                }
            }
        }

        /// <summary>
        /// 按照从【子类 => 祖先】的顺序查找符合条件的第一个属性。
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>符合条件的属性元数据</returns>
        public PropertyInfoX GetProperty(string name)
        {
            PropertyInfoX[] props = null;
            if (propertyMetaDic.TryGetValue(name, out props))
            {
                return props[0];
            }
            return null;
        }

        private void InitProperties()
        {
            Type t = Meta;
            var propertyMetas = new List<PropertyInfoX>();
            do
            {
                propertyMetas.AddRange(
                    from p in t.GetProperties(bindingAttr)
                    select new PropertyInfoX(p)
                );
                t = t.BaseType;
            } while (t != null && t != typeof(object));

            propertyMetaDic = propertyMetas.GroupBy(p => p.Name).ToDictionary(p => p.Key, p => p.ToArray());
        }
        #endregion
    }
}
