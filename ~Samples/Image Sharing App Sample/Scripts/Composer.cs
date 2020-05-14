using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF.Examples.Unigram{
    public class Composer : MonoBehaviour {
        public NotificationListView activityListView;
        [ReorderableList] public List<NotificationModel> activityModels;

        public ProfileView profileView;
        public ProfileModel profileModel;

        public void Start() {
            activityListView.ItemsSource.AddFrom(activityModels);
            profileView.Model = profileModel;
        }
    }
}
