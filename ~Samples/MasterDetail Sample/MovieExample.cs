using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class MovieExample : MonoBehaviour {
        public MasterDetailPage page;

        public Movie[] models;

        void Start() {
            page.IsDetailPageOpen = false;

            var listView = page.Master.Content as MovieListView;

            foreach (var model in models)
                listView.ItemsSource.Add(model);

            listView.OnClick += (sender, args) => {
                page.IsDetailPageOpen = true;
                (page.Detail.Content.GetSubView("Panel") as MovieView).Context = sender as Movie;
            };
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
                page.IsDetailPageOpen = false;
        }
    }
}
