using System;


namespace Adrenak.UPF.Examples {
    
    public class ContactListView : ListView<ContactCellViewModel, ContactCellView> {
        public event Action<ContactCellViewModel> OnCall;
        public event Action<ContactCellViewModel> OnDelete;
        public event Action<ContactCellViewModel> OnClick;

        protected override void Init(ContactCellViewModel cell) {
            cell.OnCall += HandleOnCall;
            cell.OnDelete += HandleOnDelete;
            cell.OnClick += HandleOnClick;
        }

        protected override void Deinit(ContactCellViewModel cell) {
            cell.OnCall -= HandleOnCall;
            cell.OnDelete -= HandleOnDelete;
            cell.OnClick -= HandleOnClick;
        }

        void HandleOnDelete(object sender, EventArgs e) {
            OnDelete?.Invoke(sender as ContactCellViewModel);
        }

        void HandleOnCall(object sender, EventArgs e) {
            OnCall?.Invoke(sender as ContactCellViewModel);
        }

        void HandleOnClick(object sender, EventArgs e) {
            OnClick?.Invoke(sender as ContactCellViewModel);
        }
    }
}
