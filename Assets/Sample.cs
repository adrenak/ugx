using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SomeNamespace;

public class Sample : MonoBehaviour {
    public TextListItem item;
    public Sprite icon;

    void Start() {
        item.Set(new Contact {
            name = "vatsal",
            description = "cool",
            displayPic = icon,
            fontColor = Color.yellow
        });

        item.OnClick += () => Debug.Log("s");
    }

    private void Person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
        Debug.Log(e.PropertyName);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Y)){
            (item.Model as Contact).name = "New Name";
            (item.Model as Contact).fontColor = Color.blue;
            item.Model.Update("name", "color");
        }
    }
}
