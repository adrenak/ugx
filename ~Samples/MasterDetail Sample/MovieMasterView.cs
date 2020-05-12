using System;
using UnityWeld.Binding;

namespace Adrenak.UPF.Examples {
    [Serializable]
    [Binding]
    public class MovieMasterView : ListView<MovieCellViewModel, MovieCellView> {
        protected override void Deinit(MovieCellViewModel cell) { }

        protected override void Init(MovieCellViewModel cell) { }
    }
}
