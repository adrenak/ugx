using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Adrenak.UPF.Examples;
using UnityEngine.UI;

namespace Adrenak.UPF.Examples{
    public class ContactListSample : MonoBehaviour {    
        public Text message;
        public ContactListView listView;
        public List<ContactCellViewModel> contacts;
        public ContactCellViewModel extraContact;

        void Start() {
            listView.InstanceNamer = instance => instance.BindingContext.Name;

            // TODO: Make an extension method for this
            foreach(var contact in contacts)
                listView.ItemsSource.Add(contact);

            listView.OnClick += (sender, e) => 
                message.text = (sender as ContactCellView).BindingContext.Name;

            listView.OnCall += contactCell => 
                message.text = "Calling " + contactCell.Name;

            listView.OnDelete += contactCell => {
                message.text = "Deleting " + contactCell.Name;
                listView.ItemsSource.Remove(contactCell);
            };

            listView.OnPullToRefresh += async (sender, args) => {
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
