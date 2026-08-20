namespace DBH.UI.Menu {
    public interface ICustomButton {
        ICover Cover { get; }

        bool StartFinished { get; }

        void HideButton() {
            Cover.Hide();
        }

        void UnHideButton() {
            Cover.UnHide();
        }

        void CoverButton() {
            Cover.Activate();
        }

        void UnCoverButton() {
            Cover.DeActivate();
        }
    }
}