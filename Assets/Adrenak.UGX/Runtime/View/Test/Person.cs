using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Adrenak.UGX;

[System.Serializable]
public class PersonState : ViewState {
    public int age;
}

public class Person : View<PersonState> {
    protected override void HandleViewStateSet() {
        throw new System.NotImplementedException();
    }
}