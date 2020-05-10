using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Adrenak.UPF.Examples;
using UnityEngine.UI;

public class ContactListSample : MonoBehaviour {
    public Text message;
    public ContactListView listView;
    public List<ContactListItemViewModel> contacts;
    public ContactListItemViewModel extraContact;

    void Start() {
        listView.LayoutGroup.spacing = 5;
        listView.BGImage.color = Color.black;

        listView.ItemsSource = contacts;
        listView.Clicked += obj => {
            message.text = (obj as ContactListItemView).Model.Name;
        };
        
        listView.OnPullToRefresh += async () => {
            await Task.Delay(2000);
            var items = listView.ItemsSource;
            items.Add(extraContact);
            listView.ItemsSource = items;
            listView.StopRefresh();
        };
    }
}
