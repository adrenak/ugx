using System.Threading.Tasks;

namespace Adrenak.UPF {
    public interface IController<T, K> where T : ViewModel {
        Task<T> Index(K k);
    }
}
