using System;

namespace Adrenak.UPF.Examples {
    [Serializable]    
    public class MovieListView : ListView<Movie_Name_Rating, Movie_Name_Rating_View> {
        public event EventHandler OnClick;

        protected override void Init(Movie_Name_Rating cell) {
            cell.OnClick += HandleOnClick;
        }

        protected override void Deinit(Movie_Name_Rating cell) {
            cell.OnClick -= HandleOnClick;
        }

        private void HandleOnClick(object sender, EventArgs e) {
            OnClick?.Invoke(sender as Movie_Name_Rating, EventArgs.Empty);
        }
    }
}
