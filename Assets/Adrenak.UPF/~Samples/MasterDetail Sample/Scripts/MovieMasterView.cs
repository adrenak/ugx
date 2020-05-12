using System;


namespace Adrenak.UPF.Examples {
    [Serializable]
    
    public class MovieMasterView : ListView<MovieCell, MovieListItemView> {
        public event EventHandler OnClick;

        protected override void Init(MovieCell cell) {
            cell.OnClick += HandleOnClick;
        }

        protected override void Deinit(MovieCell cell) {
            cell.OnClick -= HandleOnClick;
        }

        private void HandleOnClick(object sender, EventArgs e) {
            OnClick?.Invoke(sender as MovieCell, EventArgs.Empty);
        }
    }
}
