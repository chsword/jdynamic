using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chsword
{
    /// <summary>
    /// 类型分析器
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class TypeParser
    {
        #region Properties
        /// <summary>
        /// 原始字符串分析结果
        /// </summary>
        private SymbolParseResult spResult = null;
        #endregion

        #region Entrance
        /// <summary>
        /// Prevents a default instance of the <see cref="TypeParser"/> class from being created.
        /// </summary>
        private TypeParser() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParser"/> class.
        /// </summary>
        /// <param name="spResult">The symbol parse result.</param>
        public TypeParser(SymbolParseResult spResult)
        {
            this.spResult = spResult;
        }
        #endregion

        #region Business Methods
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (spResult.PeekNextIs("{"))
                return GetObject();
            else if (spResult.PeekNextIs("["))
                return GetArray();
            else
            {
                var obj = spResult.Next();
                var value = obj.Value;

                if (value[0] == '"' || value[0] == '\'')
                {
                    var content = value.GetContent();

                    DateTime time = DateTime.MinValue;
                    if (DateTime.TryParse(content, out time))
                        return time;
                    else
                        return content;
                }
                else if (value == "true")
                    return true;
                else if (value == "false")
                    return false;
                else if (value == "null")
                    return null;
                else if (value.IndexOf('.') == -1)
                    return int.Parse(value);
                else
                    return double.Parse(value);
            }
        }

        /// <summary>
        /// Resolves the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static object Resolve(string json)
        {
            return new TypeParser(SymbolParser.Build(json)).GetValue();
        }
        #endregion

        #region Private Methods
        private object GetObject()
        {
            spResult.NextIs("{", true);

            var data = new Dictionary<string, object>();
            while (!spResult.PeekNextIs("}"))
            {
                var propName = spResult.Next().Value.GetContent();
                spResult.NextIs(":", true);
                var value = GetValue();

                data.Add(propName, value);
                if (spResult.PeekNextIs(","))
                    spResult.Next();
            }
            spResult.Next();

            return data;
        }

        private object GetArray()
        {
            spResult.NextIs("[", true);

            var data = new List<object>();
            while (!spResult.PeekNextIs("]"))
            {
                data.Add(GetValue());

                if (spResult.PeekNextIs(","))
                    spResult.Next();
            }
            spResult.Next();

            return data.ToArray();
        }
        #endregion
    }
}
