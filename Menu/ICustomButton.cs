namespace DBH.UI.Menu {
    public interface ICustomButton {
        Cover Cover { get; }

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