using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
    public class ContactCellView : View<ContactCellViewModel> {
        public void Call() {
            BindingContext.Call();
        }

        public void Delete() {
            BindingContext.Delete();
        }
    }
}