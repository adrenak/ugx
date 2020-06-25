using System;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class Cache<K> {
        /// <summary>
        /// See the implementation class to see the type of object to be passed
        /// </summary>
        public abstract Task Init(object obj = null);

        /// <summary>
        /// See the implementation class to see the type of object to be passed
        /// </summary>
        public abstract void Get(object obj, Action<K> onSuccess, Action<Exception> onFailure);

        /// <summary>
        /// See the implementation class to see the type of object to be passed
        /// </summary>
        public abstract Task<K> Get(object obj);

        /// <summary>
        /// See the implementation class to see the type of object to be passed
        /// </summary>
        public abstract void Free(object obj, Action onSuccess, Action<Exception> onFailure);

        /// <summary>
        /// See the implementation class to see the type of object to be passed
        /// </summary>
        public abstract Task Free(object obj);
    }
}