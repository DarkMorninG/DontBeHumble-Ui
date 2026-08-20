namespace DBH.UI.Menu {
    public interface ICover {
        void Hide();
        void UnHide();
        void Activate();
        void DeActivate();
        bool CoverEnabled { get; }
    }
}