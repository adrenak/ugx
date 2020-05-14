using System;

namespace Adrenak.UPF.Examples {    
    public class ContactListView : ListView<ContactModel, ContactView> {
        public event Action<ContactModel> OnCall;
        public event Action<ContactModel> OnDelete;

        protected override void Init(ContactModel cell) {
            cell.OnCall += HandleOnCall;
            cell.OnDelete += HandleOnDelete;
        }

        protected override void Deinit(ContactModel cell) {
            cell.OnCall -= HandleOnCall;
            cell.OnDelete -= HandleOnDelete;
        }

        void HandleOnDelete(object sender, EventArgs e) {
            OnDelete?.Invoke(sender as ContactModel);
        }

        void HandleOnCall(object sender, EventArgs e) {
            OnCall?.Invoke(sender as ContactModel);
        }
    }
}
