using UnityEngine;

namespace Adrenak.UPF.Examples{
    public class DriverForMovieMasterDetailSample : MonoBehaviour {
        public MovieMasterDetailPage page;

        public MovieCellViewModel[] models;

        void Start() {
            var listView = (page.Master as ContentPage).Content as ListView<MovieCellViewModel, MovieCellView>;

            foreach (var model in models)
                listView.ItemsSource.Add(model);

            listView.OnClick += (sender, args) => {
                (page.Detail.Content as MovieCellView).BindingContext = (sender as MovieCellViewModel);
            };
        }
    }
}
