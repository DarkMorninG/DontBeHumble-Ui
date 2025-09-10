using DBH.Attributes;
using DBH.UI.Controller;
using DBH.UI.Menu.Commit;

namespace DBH.UI.Menu.Abort {
    public class CloseAllMenuOnCommit : CommitExtension {
        [Grab]
        private IMenuUIController menuUIController;

        public override void OnCommit() {
            menuUIController.Close();
        }
    }
}