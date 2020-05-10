using System.Collections.Generic;
using UnityEngine;
using Adrenak.UPF.Examples;

public class ContactListSample : MonoBehaviour {
    public ContactListView listView;
    public List<ContactListItemViewModel> contacts;

    void Start() {
        listView.ItemsSource = contacts;
        listView.Clicked += obj => {
            Debug.Log((obj as ContactListItemView).Model.Name);
        };
    }
}
