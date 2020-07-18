using System.Threading.Tasks;

namespace Adrenak.UPF {
    public class FadePopupTweener : PopupTweener {
        override protected async Task OnPopupOpen(){
            await tweener.MoveIn();
            tweener.FadeInAndForget();
        }

        override protected async Task OnPopupClose(){
            await tweener.FadeOut();
            tweener.MoveOutAndForget();
        }
    }
}
