using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
    public class ContactCellView : ListItemView<ContactCellViewModel> {
        public void Call() {
            BindingContext.Call();
        }

        public void Delete() {
            BindingContext.Delete();
        }
    }
}