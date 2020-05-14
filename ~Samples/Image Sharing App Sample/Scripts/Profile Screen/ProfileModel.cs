using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adrenak.UPF.Examples.Unigram {
    [Serializable]
    public class ProfileModel : Model {
        [SerializeField] ProfileSummaryModel profileSummary;
        public ProfileSummaryModel ProfileSummary {
            get => profileSummary;
            set => Set(ref profileSummary, value);
        }

        [SerializeField] List<PostPreviewModel> postPreviews = new List<PostPreviewModel>();
        public List<PostPreviewModel> PostPreviews {
            get => postPreviews;
            set => Set(ref postPreviews, value);
        }
    }
}
