using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {  
    public class ContactView : View<ContactModel> {
#pragma warning disable 0649
        [SerializeField] Text nameDisplay;
        [SerializeField] Text statusDisplay;
#pragma warning restore 0649

        public void Call() {
            Model.Call();
        }

        public void Delete() {
            Model.Delete();
        }

        protected override void InitializeView() {
            nameDisplay.text = Model.Name;
            statusDisplay.text = Model.Status;
        }

        protected override void ListenToView() {
            // This isn't a 2 way binding
        }

        protected override void OnModelPropertyChanged(string propertyName) {
            switch(propertyName){
                case nameof(Model.Name):
                    nameDisplay.text = Model.Name;
                    break;
                case nameof(Model.Status):
                    statusDisplay.text = Model.Status;
                    break;
            }
        }
    }
}