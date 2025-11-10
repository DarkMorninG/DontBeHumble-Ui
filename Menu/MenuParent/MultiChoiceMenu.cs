using System;
using System.Collections.Generic;
using System.Linq;
using BetterCoroutine;
using BetterCoroutine.AwaitRuntime;
using DBH.Attributes;
using DBH.UI.Controller;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vault;
using Vault.BetterCoroutine;
using Vault.Selector;
using Vault.Util;

namespace DBH.UI.Menu.MenuParent {
    public class MultiChoiceMenu : IMultiChoiceMenu {
        [Grab]
        private IMenuUIController _menuUIController;

        [AssetsOnly]
        [SerializeField]
        private GameObject pointerPrefab;

        [SerializeField]
        private GameObject scrollView;

        [SerializeField]
        private MenuDirection menuDirection = MenuDirection.Vertical;

        [SerializeField]
        [DisableIf("@" + nameof(scrollView) + " == null")]
        private float scrollSpeed;

        [FormerlySerializedAs("pointParent")]
        [SerializeField]
        private Transform pointerParent;

        private GameObject _createdPointer;

        private List<MenuPoint> _menuPoints = new();

        private ScrollRect _scrollRect;

        public ListSelector<MultiSelectDto> ListSelector { get; private set; }

        public override IExecutableMenu CurrentMenu => ListSelector.Selected.ExecutableMenu;

        public MenuDirection MenuDirection {
            get => menuDirection;
            set => menuDirection = value;
        }

        public void Initialize(Action onCompletion = null, MultiSelectDto toSelect = null) {
            if (scrollView != null) {
                _scrollRect = GetComponent<ScrollRect>(scrollView);
            }

            _menuPoints.Clear();
            CreateMenuPoints(AllChildren(),
                list => {
                    _menuPoints = list;
                    var multiSelectDtos = _menuPoints.Where(point => point.Selectable)
                        .Select(o =>
                            new MultiSelectDto(o.Button, o.Target.GetComponent<IExecutableMenu>(), o.Target))
                        .OrderByDescending(multiSelectDto => new HierarchySortable(multiSelectDto.GameObject))
                        .ToList();

                    ListSelector = new ListSelector<MultiSelectDto>(multiSelectDtos,
                        toSelect ?? multiSelectDtos.Last());
                    onCompletion?.Invoke();
                });
        }

        public override void ActivateMenu() {
            var lastSelectedItem = ListSelector?.Selected;
            Initialize(() => {
                    if (!ListSelector.CurrentList.IsEmpty()) {
                        if (_createdPointer != null) {
                            _createdPointer.SetActive(true);
                        } else {
                            var selectedTransform = ListSelector.Selected.GameObject.transform;
                            _createdPointer = CreateGameObject(pointerPrefab,
                                selectedTransform.position,
                                selectedTransform.rotation,
                                pointerParent != null ? pointerParent : transform.parent);
                        }

                        ListSelector.CurrentList.ForEach(multiSelectDto => multiSelectDto.ExecutableMenu.DeSelected());
                    }

                    ChangePointer(ListSelector.Selected, null);

                    ListSelector.OnSelectionChange += ChangePointer;
                    Open();
                },
                GetLastSelectedItem(lastSelectedItem));
        }

        private static MultiSelectDto GetLastSelectedItem(MultiSelectDto lastSelectedItem) {
            if (lastSelectedItem == null) return null;
            if (lastSelectedItem.CustomButton.Cover.CoverEnabled) return null;
            return lastSelectedItem.GameObject == null ? null : lastSelectedItem;
        }

        public override int MenuPointCount() {
            return 1;
        }

        public override void DeActivateMenu() {
            if (_createdPointer != null) _createdPointer.SetActive(false);
            if (ListSelector != null) ListSelector.OnSelectionChange -= ChangePointer;
        }

        public override void Reload(Action onFinished = null) {
            if (ListSelector == null) return;
            ListSelector.OnSelectionChange -= ChangePointer;
            Initialize(() => {
                    ListSelector.OnSelectionChange += ChangePointer;
                    if (ListSelector.CurrentList.IsEmpty()) {
                        _createdPointer.SetActive(false);
                    }

                    ChangePointer(ListSelector.Selected, null);
                    onFinished?.Invoke();
                },
                ListSelector.Selected);
        }

        protected override bool IncreaseVerticalInternal() {
            ListSelector?.Selected.ExecutableMenu.DirectionInput(ExecutableMenu.Direction.Up);
            if (menuDirection == MenuDirection.Vertical) {
                ListSelector?.Increase();
                if (ListSelector?.CurrentList.Count > 1) {
                    return true;
                }
            }

            return false;
        }

        protected override bool DecreaseVerticalInternal() {
            ListSelector?.Selected.ExecutableMenu.DirectionInput(ExecutableMenu.Direction.Down);
            if (menuDirection == MenuDirection.Vertical) {
                ListSelector?.Decrease();
                if (ListSelector?.CurrentList.Count > 1) {
                    return true;
                }
            }
            return false;
        }

        protected override bool IncreaseHorizontalInternal() {
            ListSelector?.Selected.ExecutableMenu.DirectionInput(ExecutableMenu.Direction.Right);
            if (menuDirection == MenuDirection.Horizontal) {
                ListSelector?.Increase();
                if (ListSelector?.CurrentList.Count > 1) {
                    return true;
                }
            }
            return false;
        }

        protected override bool DecreaseHorizontalInternal() {
            ListSelector?.Selected.ExecutableMenu.DirectionInput(ExecutableMenu.Direction.Left);
            if (menuDirection == MenuDirection.Horizontal) {
                ListSelector?.Decrease();
                if (ListSelector?.CurrentList.Count > 1) {
                    return true;
                }
            }

            return false;
        }


        public override void CoverAll() {
            _menuPoints.ForEach(m => m.Button.CoverButton());
        }

        public override void UnCoverAll() {
            _menuPoints.ForEach(m => m.Button.UnCoverButton());
        }

        public override void Destroy() {
            Destroy(gameObject);
        }

        protected override void CommitInternal(AudioPlayerDto audioPlayerDto) {
            ListSelector.Selected.ExecutableMenu.Commit(audioPlayerDto);
        }

        public override void CommitProgress(int progress) {
            ListSelector?.Selected.ExecutableMenu.CommitProgress(progress);
        }

        public override void CommitProgressCompleted() {
            ListSelector?.Selected.ExecutableMenu.CommitProgressCompleted();
        }

        public override void CommitProgressAborted() {
            ListSelector?.Selected.ExecutableMenu.CommitProgressAborted();
        }

        protected override void AbortInternal(AudioPlayerDto audioPlayerDto) {
            ListSelector.Selected.ExecutableMenu.Abort(audioPlayerDto);
        }

        protected override void CloseInternal() {
            if (_createdPointer != null) _createdPointer.SetActive(false);
            _menuPoints.ForEach(g => g.Button.HideButton());
        }

        public override void Open() {
            if (_createdPointer != null) _createdPointer.SetActive(true);

            _menuPoints.ForEach(g => g.Button.UnHideButton());
        }

        public override void CoverExcept(GameObject expect) {
            _menuPoints.ForEach(m => {
                if (!m.Target.Equals(expect)) m.Button.CoverButton();
            });
        }

        private void ChangePointer(MultiSelectDto newSelected, MultiSelectDto oldSelected) {
            var rectTransform = newSelected.GameObject.transform as RectTransform;
            _createdPointer.transform.localScale = newSelected.GameObject.transform.localScale;
            var createdPointerTransform = _createdPointer.transform as RectTransform;
            createdPointerTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            if (_scrollRect != null) {
                Canvas.ForceUpdateCanvases();
                UnityAsyncRuntime.Create(_scrollRect.FocusOnItemCoroutine(rectTransform, scrollSpeed))
                    .AndAfterFinishDo(() => {
                        var position = newSelected.GameObject.transform.position;
                        if (menuDirection == MenuDirection.Horizontal) {
                            position = new Vector3(position.x, position.y - rectTransform.rect.height / 2, position.z);
                        }

                        _createdPointer.transform.DOMove(position, .1f);
                    });
            } else {
                var position = newSelected.GameObject.transform.position;
                if (menuDirection == MenuDirection.Horizontal) {
                    // position = new Vector3(position.x, position.y - rectTransform.rect.height / 2, position.z);
                }

                _createdPointer.transform.DOMove(position, .1f);
            }

            oldSelected?.ExecutableMenu.DeSelected();
            newSelected.ExecutableMenu.Selected();
            MenuButtonSelected();
        }

        [ContextMenu("Add Children")]
        public List<GameObject> AllChildren() {
            return _menuPoints.IsEmpty()
                ? gameObject.GetChildren().ToList()
                : _menuPoints.Select(point => point.Target).ToList();
        }

        private void CreateMenuPoints(List<GameObject> allMenuPoints, Action<List<MenuPoint>> onFinished) {
            List<MenuPoint> createdMenuPoints = new();
            List<IAwaitRuntime> allRunTimes = new();
            allMenuPoints.Remove(_createdPointer);

            allMenuPoints.ForEach(m => {
                MenuPoint menuPoint;
                var customButton = m.GetComponent<ICustomButton>();
                if (customButton == null) return;
                allRunTimes.Add(IAwaitRuntime.WaitUntil(() => customButton.StartFinished,
                    () => {
                        menuPoint = customButton.Cover.CoverEnabled
                            ? new MenuPoint(m, false, customButton)
                            : new MenuPoint(m, true, customButton);
                        createdMenuPoints.Add(menuPoint);
                    }));
            });
            IAwaitRuntime.WaitUntil(() => allRunTimes.All(runtime => runtime.IsFinished),
                () => {
                    if (createdMenuPoints.IsEmpty()) {
                        _menuUIController.GoBack();
                    } else {
                        onFinished?.Invoke(createdMenuPoints);
                    }
                });
        }

        public void OnDisable() {
            DeActivateMenu();
        }
    }
}