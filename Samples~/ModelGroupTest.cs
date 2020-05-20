using Adrenak.UPF.Examples;
using Adrenak.UPF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections.ObjectModel;

public class ModelGroupTest : MonoBehaviour {
    public List<ContactModel> models;

    // Start is called before the first frame update
    IEnumerator Start() {
        void onCall(object sender, EventArgs e) =>
            Debug.Log((sender as ContactModel).Name + " calling");

        void onDelete(object sender, EventArgs e) =>
            Debug.Log((sender as ContactModel).Name + " deleted");

        var group = new ModelGroup<ContactModel>(models,
            new Action<ContactModel>[] {
                x => x.OnCall += onCall,
                x => x.OnDelete+= onDelete
            },
            new Action<ContactModel>[] {
                x => x.OnCall -= onCall,
                x => x.OnDelete -= onDelete
            }
        );

        //models.ForEach(x => x.OnCall += X_OnCall);
        yield return new WaitForSeconds(1);
        models[1].Call();
        models[1].Delete();

        var old = models[1];
        yield return new WaitForSeconds(1);

        group.Models.RemoveAt(1);
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
