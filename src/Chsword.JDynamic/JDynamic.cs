using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace Chsword
{
    /// <summary>
    /// Json Dynamic Class
    /// </summary>
    [DebuggerStepThrough]
    public sealed class JDynamic : DynamicObject
    {
        #region Private Fields
        private readonly ConcurrentDictionary<string, object> cacheMember = new ConcurrentDictionary<string, object>();
        #endregion

        #region Business Properties
        /// <summary>
        /// Gets or sets the deserialize function.
        /// </summary>
        public static Func<string, object> DeserializeFunc = null;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; private set; }
        #endregion

        #region Entrance
        /// <summary>
        /// Prevents a default instance of the <see cref="JDynamic"/> class from being created.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private JDynamic(ref object obj)
        {
            Value = obj;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="JDynamic"/> class from being created.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public JDynamic(object obj)
        {
            Value = obj;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JDynamic"/> class.
        /// </summary>
        /// <param name="json">The json.</param>
        public JDynamic(string json)
        {
            Value = DeserializeFunc == null ? TypeParser.Resolve(json) : DeserializeFunc(json);
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!TryGetField(binder.Name, out result))
                return base.TryGetMember(binder, out result);
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes[0]"/> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var key = indexes[0];
            var isArray = Value is object[];
            if (key is int && isArray)
            {
                var keyName = key.ToString();
                if (!cacheMember.TryGetValue(keyName, out result))
                {
                    var item = Value.To<object[]>()[key.To<int>()];
                    result = GetValue(item);
                    cacheMember.TryAdd(keyName, result);
                }
                return true;
            }
            else if (key is string && !isArray && TryGetField(key.To<string>(), out result))
                return true;

            return base.TryGetIndex(binder, indexes, out result);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return new ObjectResolver().Resolve(this.Value);
        }
        #endregion

        #region Private Methods
        private bool TryGetField(string name, out object result)
        {
            if (cacheMember.TryGetValue(name, out result))
                return true;

            if (Value is IDictionary<string, object> && Value.To<IDictionary<string, object>>().TryGetValue(name, out result))
            {
                result = GetValue(result);
                cacheMember.TryAdd(name, result);
                return true;
            }
            else if (Value is object[] && (name == "Count" || name == "Length"))
            {
                result = Value.To<object[]>().Length;
                cacheMember.TryAdd(name, result);
                return true;
            }
            else if (name == "Value")
            {
                result = Value;
                cacheMember.TryAdd(name, result);
                return true;
            }

            return false;
        }

        private object GetValue(object obj)
        {
            object data;
            if (obj is string || obj is ValueType)
                data = obj;
            else if (obj is IDictionary<string, object>)
                data = new JDynamic(ref obj);
            else if (obj is object[])
                data = obj.To<object[]>().Select(GetValue).ToArray();
            else
                throw new ApplicationException("Error type.");
            return data;
        }
        #endregion
    }
}
