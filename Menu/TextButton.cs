using UnityEngine;

namespace DBH.UI.Menu {
    public class TextButton : ExecutableMenu {
        [SerializeField]
        private Cover cover;
        
        public override Cover Cover => cover;


        public void HideButton() {
            cover.Hide();
        }

        public void UnHideButton() {
            cover.UnHide();
        }

        public void CoverButton() {
            cover.Activate();
        }

        public void UnCoverButton() {
            cover.DeActivate();
        }
    }
}