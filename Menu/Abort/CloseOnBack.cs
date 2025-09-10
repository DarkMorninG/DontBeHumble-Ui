using DBH.Attributes;
using DBH.UI.Controller;

namespace DBH.UI.Menu.Abort {
    public class CloseOnBack : AbortExtension {
        
        [Grab]
        private IMenuUIController _menuUIController;

        public override void OnAbort() {
            _menuUIController.GoBack();
        }
    }
}