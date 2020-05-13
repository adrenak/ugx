using System;

namespace Adrenak.UPF.Examples {    
    public class ContactListView : ListView<ContactVM, ContactView> {
        public event Action<ContactVM> OnCall;
        public event Action<ContactVM> OnDelete;

        protected override void Init(ContactVM cell) {
            cell.OnCall += HandleOnCall;
            cell.OnDelete += HandleOnDelete;
        }

        protected override void Deinit(ContactVM cell) {
            cell.OnCall -= HandleOnCall;
            cell.OnDelete -= HandleOnDelete;
        }

        void HandleOnDelete(object sender, EventArgs e) {
            OnDelete?.Invoke(sender as ContactVM);
        }

        void HandleOnCall(object sender, EventArgs e) {
            OnCall?.Invoke(sender as ContactVM);
        }
    }
}
