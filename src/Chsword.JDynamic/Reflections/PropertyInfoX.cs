using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;

namespace Chsword.Reflections
{
    [Serializable]
    [DebuggerStepThrough]
    [DebuggerDisplay("[Name = {Name}, DeclaringType = {Meta.DeclaringType}]")]
    public class PropertyInfoX : MetaXBase<PropertyInfo>, IValue
    {
        internal PropertyInfoX(PropertyInfo property)
        {
            Meta = property;

            GetValue = MarkDelegate();
        }

        #region IValue 成员

        public Func<object, object> GetValue { get; private set; }

        #endregion

        private Func<object, object> MarkDelegate()
        {
            var p = Expression.Parameter(typeof(object), "p");
            var pConverted = Expression.Convert(p, Meta.DeclaringType);

            var memberExpr = Expression.Property(pConverted, Meta);
            var memberExprConverted = Expression.Convert(memberExpr, typeof(object));

            var lambda = Expression.Lambda<Func<object, object>>(memberExprConverted, p);
            return lambda.Compile();
        }
    }
}
