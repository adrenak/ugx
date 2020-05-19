using Adrenak.UPF.Examples;
using Adrenak.UPF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelGroupTest : MonoBehaviour {
    public List<ContactModel> models;

    // Start is called before the first frame update
    IEnumerator Start() {
        var group = new ModelGroup<ContactModel>(
            new List<Action<ContactModel>>{
                x => x.OnCall += (sender, e) => Debug.Log("Calling " + (sender as ContactModel).Name),
                x => x.OnDelete += (sender, e) => Debug.Log("Delete " + (sender as ContactModel).Name)
            },
            new List<Action<ContactModel>>{
                x => x.OnCall -= (sender, e) => Debug.Log("Calling " + (sender as ContactModel).Name),
                x => x.OnDelete -= (sender, e) => Debug.Log("Delete " + (sender as ContactModel).Name)
            }
        );

        group.Models.AddRange(models);

        //models.ForEach(x => x.OnCall += X_OnCall);
        yield return new WaitForSeconds(1);
        models[1].Call();
        var old = models[1];
        yield return new WaitForSeconds(1);
        models.RemoveAt(1);
        yield return new WaitForSeconds(1);
        old.Call();
    }

    private void X_OnCall(object sender, EventArgs e) {
        throw new NotImplementedException();
    }

    void Handle(object sender, EventArgs e) {
        Debug.Log((sender as ContactModel).Name);
    }
}
