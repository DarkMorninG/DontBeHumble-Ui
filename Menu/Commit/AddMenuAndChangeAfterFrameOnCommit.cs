using DBH.Attributes;
using DBH.UI.Controller;
using UnityEngine;
using Vault.BetterCoroutine;

namespace DBH.UI.Menu.Commit {
    public class AddMenuAndChangeOnAfterFrameCommit : CommitExtension {
        [Grab]
        private IMenuUIController menuUIController;

        [SerializeField]
        private MenuParent.MenuParent menuParent;


        public override void OnCommit(ItemHolder itemHolder) {
            if (menuParent != null) {
                UnityAsyncRuntime.WaitForEndOfFrame(() => menuUIController.AddMenuAndChange(menuParent));
            } else if (itemHolder.Contains<MenuParent.MenuParent>()) {
                UnityAsyncRuntime.WaitForEndOfFrame(() =>
                    menuUIController.AddMenuAndChange(itemHolder.Item<MenuParent.MenuParent>()));
            }
        }
    }
}