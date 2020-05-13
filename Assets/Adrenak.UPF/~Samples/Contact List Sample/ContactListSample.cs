using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class ContactListSample : MonoBehaviour {    
        public Text message;
        public ContactListView listView;
        public List<ContactVM> contacts;
        public ContactVM extraContact;

        void Start() {
            listView.InstanceNamer = instance => instance.Context.Name;

            // TODO: Make an extension method for this
            foreach(var contact in contacts)
                listView.ItemsSource.Add(contact);

            listView.OnItemSelected += (source, e) =>
                message.text = (e.Item as ContactVM).Name;

            listView.OnCall += contactCell => 
                message.text = "Calling " + contactCell.Name;

            listView.OnDelete += contactCell => {
                message.text = "Deleting " + contactCell.Name;
                listView.ItemsSource.Remove(contactCell);
            };

            listView.OnPulledToRefresh += async (sender, args) => {
                await Task.Delay(200);
                listView.ItemsSource.Add(extraContact);
                listView.StopRefresh();
            };
        }

        public void AddExtra(){
            listView.ItemsSource.Add(extraContact);
        }
    }
}
