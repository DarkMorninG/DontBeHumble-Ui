using UnityEngine;

namespace DBH.UI.Menu.MenuParent {
    public abstract class IMultiChoiceMenu : MenuParent {
        public abstract void CoverExcept(GameObject expect);
    }
}