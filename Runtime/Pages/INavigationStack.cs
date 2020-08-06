using System.Collections.Generic;

namespace Adrenak.UPF {
    public interface INavigationStack {
        List<Page> History { get; }
        void Push(Page page);
        void Pop();
    }
}

