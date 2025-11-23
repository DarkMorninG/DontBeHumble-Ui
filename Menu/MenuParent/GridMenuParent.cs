using System.Collections.Generic;
using System.Linq;
using BetterCoroutine.AwaitRuntime;
using DBH.UI.Controller;
using UnityEngine;
using UnityEngine.UI;
using Vault;
using Vault.BetterCoroutine;
using Vault.Selector;

namespace DBH.UI.Menu.MenuParent {
    public class GridMenuParent : MenuParent {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private bool roundRobin = true;

        private GridSelector<MultiSelectDto> gridSelector;

        public override IExecutableMenu CurrentMenu => gridSelector.CurrentSelected.ExecutableMenu;

        public override void ActivateMenu() {
            MultiSelectDto lastSelected = null;
            if (gridSelector != null) {
                lastSelected = gridSelector.CurrentSelected;
            }

            var gridLayoutGroup = GetComponent<GridLayoutGroup>();
            var gridPointer = GetComponent<IGridPointer>();
            if (gridLayoutGroup != null) {
                gridPointer.PointerSize = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
            }

            IAwaitRuntime.WaitForEndOfFrame(() => {
                CreateMenuPoints(gameObject.GetChildren().Where(o => o.GetComponent<ICustomButton>() != null).ToList(),
                    list => {
                        var gridItems = CreateGridItems(list);
                        gridSelector = lastSelected != null && lastSelected.GameObject != null
                            ? new GridSelector<MultiSelectDto>(gridItems, lastSelected) { RoundRobin = roundRobin }
                            : new GridSelector<MultiSelectDto>(gridItems) { RoundRobin = roundRobin };
                        gridPointer.ActivatePointer(gridSelector, scrollRect);
                    });
            });
        }

        public override int MenuPointCount() {
            return gridSelector.GridItems.Count;
        }


        public override void DeActivateMenu() {
            GetComponent<IGridPointer>().DeActivePointer(gridSelector);
        }

        public override void Destroy() {
        }

        protected override void CommitInternal(AudioPlayerDto audioPlayerDto) {
            gridSelector.CurrentSelected.ExecutableMenu.Commit(audioPlayerDto);
        }

        public override void CommitProgress(int progress) {
            gridSelector?.CurrentSelected.ExecutableMenu.CommitProgress(progress);
        }

        public override void CommitProgressCompleted() {
            gridSelector?.CurrentSelected.ExecutableMenu.CommitProgressCompleted();
        }

        public override void CommitProgressAborted() {
            gridSelector?.CurrentSelected.ExecutableMenu.CommitProgressAborted();
        }

        protected override void AbortInternal(AudioPlayerDto audioPlayerDto) {
            gridSelector.CurrentSelected.ExecutableMenu.Abort(audioPlayerDto);
        }

        protected override bool IncreaseVerticalInternal() {
            gridSelector.Up();
            return gridSelector.GridItems.Count > 1;
        }

        protected override bool DecreaseVerticalInternal() {
            gridSelector.Down();
            return gridSelector.GridItems.Count > 1;
        }

        protected override bool IncreaseHorizontalInternal() {
            gridSelector.Left();
            return gridSelector.GridItems.Count > 1;
        }

        protected override bool DecreaseHorizontalInternal() {
            gridSelector.Right();
            return gridSelector.GridItems.Count > 1;
        }


        private static List<GridItem<MultiSelectDto>> CreateGridItems(List<MenuPoint> list) {
            List<GridItem<MultiSelectDto>> gridItems = new();
            var columnGrouped = list.GroupBy(point => point.Target.transform.position.y)
                .OrderByDescending(points => points.Key)
                .ToDictionary(points => points.Key,
                    points => points.OrderBy(point => point.Target.transform.position.x).ToList());

            var currentKey = columnGrouped.First().Key;
            var currentRow = 0;
            foreach (var (key, value) in columnGrouped) {
                if (key != currentKey) {
                    currentRow++;
                    currentKey = key;
                }

                gridItems.AddRange(
                    value.Select(point => new MultiSelectDto(point.Button,
                            point.Target.GetComponent<IExecutableMenu>(),
                            point.Target))
                        .Select((multiSelectDto, i) =>
                            new GridItem<MultiSelectDto>(currentRow, i, multiSelectDto)));
            }

            return gridItems;
        }
    }
}