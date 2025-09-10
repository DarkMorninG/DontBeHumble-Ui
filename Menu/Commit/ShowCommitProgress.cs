using DG.Tweening;
using UnityEngine;

namespace DBH.UI.Menu.Commit {
    public class ShowCommitProgress : CommitExtension {
        [SerializeField]
        private GameObject progressIndicator;

        private bool _disable;

        public bool Disable {
            get => _disable;
            set => _disable = value;
        }


        public override void OnCommitProgress(int progress) {
            if (Disable) return;
            progressIndicator.transform.DOScaleX((float)progress / 100, .01f);
        }

        public override void OnCommitProgressAborted() {
            if (Disable) return;
            progressIndicator.transform.DOScaleX(0, .1f);
        }


        public override void OnCommitProgressFinished() {
            if (Disable) return;
            progressIndicator.transform.DOScaleX(0, .1f);
        }
    }
}