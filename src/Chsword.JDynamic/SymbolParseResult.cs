using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Chsword
{
    /// <summary>
    /// Symbol Parse Result
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DebuggerDisplay("{ToString()}")]
    internal class SymbolParseResult : ReadOnlyCollection<KeyValuePair<int, string>>
    {
        #region Private Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _maxIndex = 0;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _lastIndex = 0;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _index = -1;
        #endregion

        #region Constuction
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolParseResult"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        internal SymbolParseResult(IList<KeyValuePair<int, string>> list)
            : base(list)
        {
            _maxIndex = list.Count - 1;
        }
        #endregion

        #region Business Properties
        /// <summary>
        /// 获取或设置当前读取索引
        /// </summary>
        public int Index
        {
            get { return _index; }
            private set
            {
                _lastIndex = _index;
                _index = value;
            }
        }
        /// <summary>
        /// 获取当前读取中的字符单元
        /// </summary>
        public KeyValuePair<int, string> Current
        {
            get
            {
                if (Index < 0 || Index > _maxIndex)
                    return new KeyValuePair<int, string>();

                return this[Index];
            }
        }
        /// <summary>
        /// 获取完整的字符串表达式
        /// </summary>
        private string StringExpression
        {
            get { return string.Join(" ", this.Select(p => p.Value)); }
        }
        #endregion

        #region Business Methods
        /// <summary>
        /// 读取下一个字符单元, 同时读取索引前进.
        /// </summary>
        /// <returns>读取得到的字符单元</returns>
        public KeyValuePair<int, string> Next()
        {
            KeyValuePair<int, string> token;
            if (TryGetElement(out token, Index + 1))
                return token;
            else
                return new KeyValuePair<int, string>();
        }

        /// <summary>
        /// 判断下一个字符单元是否是指定的类型, 同时读取索引前进.
        /// </summary>
        /// <param name="text">期待得到的字符单元.</param>
        /// <param name="throwIfNot">如果设置为 <c>true</c> 表示抛出异常. 默认为 <c>false</c> 表示不抛出异常.</param>
        /// <returns><c>true</c> 表示读取的单元类型和期待的单元类型一致; 否则返回 <c>false</c> .</returns>
        public bool NextIs(string text, bool throwIfNot = false)
        {
            var result = Next().Value == text;
            if (!result && throwIfNot)
                throw new ApplicationException(string.Format("next is not {0}", text));
            return result;
        }

        /// <summary>
        /// 尝试读取下一个字符单元, 但并不前进.
        /// </summary>
        /// <param name="count">尝试读取的当前字符单元的后面第几个单元, 默认为后面第一个单元.</param>
        /// <returns>读取得到的字符单元.</returns>
        public KeyValuePair<int, string> PeekNext(int count = 1)
        {
            KeyValuePair<int, string> token;
            if (PeekGetElement(out token, Index + count))
                return token;
            else
                return new KeyValuePair<int, string>();
        }

        /// <summary>
        /// 判断下一个字符单元是否是指定的类型, 但读取索引不前进.
        /// </summary>
        /// <param name="text">期待得到的字符单元类型.</param>
        /// <param name="count">判断当前字符后面第几个是指定的字符单元类型, 默认值为 1 .</param>
        /// <param name="throwIfNot">如果设置为 <c>true</c> 表示抛出异常. 默认为 <c>false</c> 表示不抛出异常.</param>
        /// <returns>
        /// 	<c>true</c> 表示读取的单元类型和期待的单元类型一致; 否则返回 <c>false</c> .
        /// </returns>
        public bool PeekNextIs(string text, int count = 1, bool throwIfNot = false)
        {
            var result = PeekNext(count).Value == text;
            if (!result && throwIfNot)
                throw new ApplicationException(string.Format("next is not {0}", text));
            return result;
        }
        #endregion

        #region Private Methods
        private bool TryGetElement(out KeyValuePair<int, string> token, int index)
        {
            bool result = PeekGetElement(out token, index);
            if (result)
                Index = index;
            return result;
        }

        private bool PeekGetElement(out KeyValuePair<int, string> token, int index)
        {
            if (index < 0 || index > _maxIndex)
            {
                token = new KeyValuePair<int, string>();
                return false;
            }
            else
            {
                token = this[index];
                return true;
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join(" ", this.TakeWhile(p => p.Key < Current.Key).Select(p => p.Value));
        }
        #endregion
    }
}
