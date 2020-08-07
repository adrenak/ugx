using System.Collections.Generic;

namespace Adrenak.UPF {
    public interface INavigationStack {
        List<View> History { get; }
        void Push(View page);
        void Pop();
    }
}

