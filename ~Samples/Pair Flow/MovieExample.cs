using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class MovieExample : MonoBehaviour {
        public PairFlow page;

        public MovieModel[] models;

        void Start() {
            page.IsShowingSecond = false;

            var listView = page.First.Content as MovieListView;

            listView.Items = models;

            listView.OnItemSelected += (sender, args) => {
                page.IsShowingSecond = true;
                (page.Second.Content.GetSubView("Movie View") as MovieView).Model = args.Item;
            };
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
                page.IsShowingSecond = false;
        }
    }
}
