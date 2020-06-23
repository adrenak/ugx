using System;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class Cache<K> {
        public abstract Task Init();
        public abstract void Get(object obj, Action<K> onSuccess, Action<Exception> onFailure);
        public abstract Task<K> Get(object obj);

        public abstract void Free(object obj);
    }
}