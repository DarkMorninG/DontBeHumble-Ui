using DBH.Attributes;
using DBH.UI.Controller;
using UnityEngine;

namespace DBH.UI.Menu.Commit {
    public class AddMenuAndChangeOnCommit : CommitExtension {
        [Grab]
        private IMenuUIController menuUIController;
        
        [SerializeField]
        private MenuParent.MenuParent menuParent;
        
        
        public override void OnCommit() {
            menuUIController.AddMenuAndChange(menuParent);
        }
    }
}