using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Adrenak.UPF.Examples {
    public class GroupExample : MonoBehaviour {
        public bool listenToEvents;
        public List<ContactModel> models;
        public ContactModel extra;
        public Transform container;
        public ContactView viewPrefab;

        IEnumerator Start() {
            ViewGroup<ContactModel, ContactView> viewGroup;

            if (listenToEvents) {
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
                viewGroup = new ViewGroup<ContactModel, ContactView>(container, viewPrefab, modelGroup);
                
                // Subscriber and Unsubscriber can also be changed during runtime and don't necessarily
                // have to be provided in the constructor. Like this:
                //viewGroup.ModelGroup.Subscriber = new Action<ContactModel>[]{
                //    x => x.OnCall += onCall,
                //    x => x.OnDelete += onDelete
                //};
                //viewGroup.ModelGroup.Unsubscriber = new Action<ContactModel>[]{
                //    x => x.OnCall -= onCall,
                //    x => x.OnDelete -= onDelete
                //};
            }
            else {
                // If we don't want to bother with events, we can simply pass the models and not have to
                // worry about registrying subscribers and unsubscribers with a ModelGroup that we have to make.
                viewGroup = new ViewGroup<ContactModel, ContactView>(container, viewPrefab, models);
            }

            yield return new WaitForSeconds(2);
            Debug.Log("Removed");

            // We remove the model from the view group
            viewGroup.ModelGroup.Models.RemoveAt(1);
            
            // We invoke a method that fires an event in the model, but we won't get it
            // As the ViewGroup stop listening to the models that are removed from it.
            models[1].Call();

            yield return new WaitForSeconds(2);
            viewGroup.ModelGroup.Models.Add(extra);
        }
    }
}
