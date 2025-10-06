namespace DBH.UI.Menu.Blocker {
    public interface ICommitBlock {
        bool Denied(ItemHolder itemHolder);
    }
}