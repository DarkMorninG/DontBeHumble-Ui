using DBH.Base;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vault;
using Vault.BetterCoroutine;
using Vault.Selector;

namespace DBH.UI.Menu.MenuParent {
    public class CaseGridPointer : DBHMono, IGridPointer {
        [AssetsOnly]
        [SerializeField]
        private GameObject pointerPrefab;


        private GameObject createdPointer;
        private ScrollRect scrollRect;

        public Vector2 PointerSize { get; set; }

        public void ActivatePointer<T>(ISelector<T> selector, ScrollRect scrollRect = null)
            where T : MultiSelectDto {
            this.scrollRect = scrollRect;
            if (createdPointer == null) {
                createdPointer = CreateGameObject(pointerPrefab, transform.parent);
            } else {
                createdPointer.SetActive(true);
            }

            createdPointer.GetComponent<RectTransform>().sizeDelta = PointerSize;
            ChangePointer(selector.CurrentSelected, default);
            selector.OnSelectionChange += ChangePointer;
        }

        public void DeActivePointer<T>(ISelector<T> selector) where T : MultiSelectDto {
            createdPointer.SetActive(false);
            selector.OnSelectionChange -= ChangePointer;
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
            createdPointer.transform.DOMove(newSelected.GameObject.transform.position, .2f);
        }
    }
}