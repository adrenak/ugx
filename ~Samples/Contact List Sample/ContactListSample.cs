using System.Collections.Generic;
using UnityEngine;
using Adrenak.UPF.Examples;
using UnityEngine.UI;

public class ContactListSample : MonoBehaviour {
    public Text message;
    public ContactListView listView;
    public List<ContactListItemViewModel> contacts;

    void Start() {
        listView.LayoutGroup.spacing = 5;
        listView.BGImage.color = Color.black;

        listView.ItemsSource = contacts;
        listView.Clicked += obj => {
            message.text = (obj as ContactListItemView).Model.Name;
        };
    }
}
