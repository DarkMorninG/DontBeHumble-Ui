using UnityEngine;
using UnityEngine.UI;
using Vault.Selector;

namespace DBH.UI.Menu.MenuParent {
    public interface IGridPointer {
        Vector2 PointerSize { get; set; }
        void ActivatePointer<T>(ISelector<T> selector, ScrollRect scrollRect = null) where T : MultiSelectDto;

        void DeActivePointer<T>(ISelector<T> selector) where T : MultiSelectDto;
    }
}