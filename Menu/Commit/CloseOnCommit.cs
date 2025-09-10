using DBH.Attributes;
using DBH.UI.Controller;

namespace DBH.UI.Menu.Commit {
    public class CloseOnCommit : CommitExtension {
        [Grab]
        private IMenuUIController _menuUIController;

        public override void OnCommit() {
            _menuUIController.GoBack();
        }
    }
}