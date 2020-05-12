using System;

namespace Adrenak.UPF.Examples {
    [Serializable]    
    public class MovieListView : ListView<Movie, MovieView> {
        public event EventHandler OnClick;

        protected override void Init(Movie cell) {
            cell.OnClick += HandleOnClick;
        }

        protected override void Deinit(Movie cell) {
            cell.OnClick -= HandleOnClick;
        }

        private void HandleOnClick(object sender, EventArgs e) {
            OnClick?.Invoke(sender as Movie, EventArgs.Empty);
        }
    }
}
