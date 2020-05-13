using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples {  
    public class ContactView : View<ContactVM> {
#pragma warning disable 0649
        [SerializeField] Text nameDisplay;
        [SerializeField] Text statusDisplay;
#pragma warning restore 0649

        public void Call() {
            Context.Call();
        }

        public void Delete() {
            Context.Delete();
        }

        protected override void OnSetContext() {
            nameDisplay.text = Context.Name;
            statusDisplay.text = Context.Status;
        }

        protected override void BindViewToContext() {
            // This isn't a 2 way binding
        }

        protected override void OnPropertyChange(string propertyName) {
            switch(propertyName){
                case nameof(Context.Name):
                    nameDisplay.text = Context.Name;
                    break;
                case nameof(Context.Status):
                    statusDisplay.text = Context.Status;
                    break;
            }
        }
    }
}