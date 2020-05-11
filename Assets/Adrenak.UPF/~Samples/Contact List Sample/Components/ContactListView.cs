using System;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Binding]
    public class ContactListView : ListView<ContactCellViewModel, ContactCellView> {
        public event Action<ContactCellViewModel> OnCall;
        public event Action<ContactCellViewModel> OnDelete;

        protected override void Init(ContactCellViewModel cell) {
            cell.OnCall += HandleOnCall;
            cell.OnDelete += HandleOnDelete;
        }

        protected override void Deinit(ContactCellViewModel cell) {
            cell.OnCall -= HandleOnCall;
            cell.OnDelete -= HandleOnDelete;
        }

        void HandleOnDelete(object sender, EventArgs e) {
            OnDelete?.Invoke(sender as ContactCellViewModel);
        }

        void HandleOnCall(object sender, EventArgs e) {
            OnCall?.Invoke(sender as ContactCellViewModel);
        }
    }
}
