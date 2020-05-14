using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class MovieExample : MonoBehaviour {
        public MasterDetailPage page;

        public MovieModel[] models;

        void Start() {
            page.IsDetailPageOpen = false;

            var listView = page.Master.Content as MovieListView;

            foreach (var model in models)
                listView.ItemsSource.Add(model);

            listView.OnItemSelected += (sender, args) => {
                page.IsDetailPageOpen = true;
                (page.Detail.Content.GetSubView("Movie View") as MovieView).Model = args.Item;
            };
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
                page.IsDetailPageOpen = false;
        }
    }
}
