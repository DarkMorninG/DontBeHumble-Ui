using System.Collections.Generic;
using DBH.UI.Menu.MenuParent;

namespace DBH.UI.Controller {
    public interface IMenuUIController {
        public delegate void ClosedMenu(MenuParent menu);

        HashSet<MenuParent> CurrentlyOpenMenus { get; }
        MenuParent CurrentMenu { get; }
        void GoBack();
        void AddMenuAndChange(MenuParent toChangeMenuParent);
        void OpenMenus();
        void HideMenus();
        void OpenOnlyLast();
        void EnableMenuInteraction();
        void DisableMenuInteraction();
        void Close();
        void OpenOnlyFirst();
        void NavigateTo(MenuParent menuParent);

        event ClosedMenu OnCloseMenu;
        void ChangeCurrentMenu(MenuParent toChangeMenuParent);
    }
}