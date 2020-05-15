using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class ContactListSample : MonoBehaviour {    
        public Text message;
        public ContactListView listView;
        public List<ContactModel> contacts;
        public ContactModel extraContact;

        void Start() {
            listView.InstanceNamer = instance => instance.Model.Name;

            // TODO: Make an extension method for this
            foreach(var contact in contacts)
                listView.Items.Add(contact);

            listView.OnItemSelected += (source, e) =>
                message.text = (e.Item as ContactModel).Name;

            listView.OnCall += contactCell => 
                message.text = "Calling " + contactCell.Name;

            listView.OnDelete += contactCell => {
                message.text = "Deleting " + contactCell.Name;
                listView.Items.Remove(contactCell);
            };

            listView.OnPulledToRefresh += async (sender, args) => {
                await Task.Delay(200);
                listView.Items.Add(extraContact);
                listView.StopRefresh();
            };
        }

        public void AddExtra(){
            listView.Items.Add(extraContact);
        }
    }
}
