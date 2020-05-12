using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class MovieExample : MonoBehaviour {
        public MovieMasterDetailPage page;

        public MovieCell[] models;

        void Start() {
            var listView = page.Master.Content;

            foreach (var model in models)
                listView.ItemsSource.Add(model);

            listView.OnClick += (sender, args) => {
                page.Detail.Content.Context = sender as MovieCell;
            };
        }
    }
}
