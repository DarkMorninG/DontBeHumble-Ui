using DBH.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vault;
using Vault.BetterCoroutine;
using Vault.Selector;

namespace DBH.UI.Menu.MenuParent {
    public class ColorGridPointer : DBHMono, IGridPointer {
        [FormerlySerializedAs("color")]
        [SerializeField]
        private Color selectorColor;

        private ScrollRect scrollRect;

        public Vector2 PointerSize { get; set; }

        public void ActivatePointer<T>(ISelector<T> selector, ScrollRect scrollRect = null)
            where T : MultiSelectDto {
            this.scrollRect = scrollRect;
            ChangePointer(selector.CurrentSelected, null);
            selector.OnSelectionChange += ChangePointer;
        }

        public void DeActivePointer<T>(ISelector<T> selector) where T : MultiSelectDto {
            selector.OnSelectionChange -= ChangePointer;
            selector.CurrentSelected.GameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }


        private void ChangePointer<T>(T newSelected, T oldSelected) where T : MultiSelectDto {
            var rectTransform = newSelected.GameObject.transform as RectTransform;
            if (scrollRect != null) {
                UnityAsyncRuntime
                    .Create(scrollRect.FocusOnItemCoroutine(rectTransform, 7f))
                    .AndAfterFinishDo(() => DisplayPointerChange(newSelected, oldSelected));
            } else {
                DisplayPointerChange(newSelected, oldSelected);
            }

            newSelected.ExecutableMenu.Selected();
            oldSelected?.ExecutableMenu.DeSelected();
        }


        private void DisplayPointerChange(MultiSelectDto newSelected, MultiSelectDto oldSelected) {
            if (oldSelected != null) {
                oldSelected.GameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }

            newSelected.GameObject.GetComponent<SpriteRenderer>().color = selectorColor;
        }
    }
}