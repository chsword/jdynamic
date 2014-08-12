using System;
using System.Diagnostics;
using System.Reflection;

namespace Chsword.Reflections
{
    /// <summary>
    /// ���Ϳ��ٳ�Ա���ʻ���
    /// </summary>
    /// <typeparam name="T">�� <paramref name="System.Reflection.MemberInfo"/> ��̳е�����</typeparam>
    [Serializable]
    [DebuggerStepThrough]
    public class MetaXBase<T> : IMetaX<T> where T : MemberInfo
    {
        #region IMetaX<T> ��Ա

        /// <summary>
        /// ��ȡ��ԱԪ���ݡ�
        /// </summary>
        public T Meta { get; protected set; }

        /// <summary>
        /// ��ȡ��ԱԪ�������ơ�
        /// </summary>
        public string Name
        {
            get { return Meta.Name; }
        }

        #endregion

        #region Override Object
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// 	<paramref name="obj"/> ����Ϊ null��
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Meta.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Meta.GetHashCode();
        }
        #endregion
    }
}