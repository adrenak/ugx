

namespace Adrenak.UPF.Examples {
    
    public class ContactCellView : View<ContactCellViewModel> {
        public void Call() {
            Context.Call();
        }

        public void Delete() {
            Context.Delete();
        }

        protected override void InitializeFromContext() {
            throw new System.NotImplementedException();
        }

        protected override void BindViewToContext() {
            throw new System.NotImplementedException();
        }

        protected override void OnPropertyChange(string propertyName) {
            throw new System.NotImplementedException();
        }
    }
}