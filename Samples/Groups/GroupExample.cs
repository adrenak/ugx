using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Adrenak.UPF.Examples {
    public class GroupExample : MonoBehaviour {
        public List<ContactModel> models;
        public Transform container;
        public ContactView viewPrefab;

        IEnumerator Start() {
            void onCall(object sender, EventArgs e) =>
                Debug.Log((sender as ContactModel).Name + " calling");

            void onDelete(object sender, EventArgs e) =>
                Debug.Log((sender as ContactModel).Name + " deleted");

            var modelGroup = new ModelGroup<ContactModel>(models,
                new Action<ContactModel>[]{
                    x => x.OnCall += onCall,
                    x => x.OnDelete += onDelete
                },
                new Action<ContactModel>[]{
                    x => x.OnCall -= onCall,
                    x => x.OnDelete -= onDelete
                }
            );

            //var viewGroup = new ViewGroup<ContactModel, ContactView>(container, viewPrefab, models, modelGroup);
            var viewGroup = new ViewGroup<ContactModel, ContactView>(container, viewPrefab, new ModelGroup<ContactModel>(models));

            yield return new WaitForSeconds(2);
            Debug.Log("Removed");
            viewGroup.ModelGroup.Models.RemoveAt(1);
            //modelGroup.Models.RemoveAt(1);
            models[1].Call();
        }
    }
}
