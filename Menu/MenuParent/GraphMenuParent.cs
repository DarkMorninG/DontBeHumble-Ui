using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vault;
using Vault.BetterCoroutine;
using Vault.Selector;

namespace DBH.UI.Menu.MenuParent {
    public class GraphMenuParent : MenuParent {
        [SerializeField]
        private ScrollRect scrollRect;

        private GraphSelector<MultiSelectDto> graphSelector;

        public override IExecutableMenu CurrentMenu => graphSelector.CurrentSelected.ExecutableMenu;

        public override void ActivateMenu() {
            MultiSelectDto lastSelected = null;
            if (graphSelector != null) {
                lastSelected = graphSelector.CurrentSelected;
            }

            UnityAsyncRuntime.WaitForEndOfFrame(() => {
                CreateMenuPoints(gameObject.GetChildren().Where(o => o.GetComponent<ICustomButton>() != null).ToList(),
                    list => {
                        graphSelector = lastSelected != null && lastSelected.GameObject != null
                            ? new GraphSelector<MultiSelectDto>(CreateGridItems(list), lastSelected)
                            : new GraphSelector<MultiSelectDto>(CreateGridItems(list));
                        GetComponent<IGridPointer>().ActivatePointer(graphSelector, scrollRect);
                    });
            });
        }

        private List<GraphNode<MultiSelectDto>> CreateGridItems(List<MenuPoint> menuPoints) {
            var graphNodes = menuPoints.Select(point =>
                    new MultiSelectDto(point.Button, point.Target.GetComponent<IExecutableMenu>(), point.Target))
                .Select(node => new GraphNode<MultiSelectDto>(node))
                .ToList();

            foreach (var graphNode in graphNodes) {
                graphNode.Value.GameObject.GetComponent<IGraphElementMenu>()
                    .GetDirections()
                    .ForEach(pair => graphNodes.FindOptional(node => node.Value.GameObject == pair.Key)
                        .IfPresent(node => graphNode.AddEdge(pair.Value, node)));
            }

            return graphNodes;
        }

        public override int MenuPointCount() {
            return graphSelector.Nodes.Count;
        }

        public override void DeActivateMenu() {
            GetComponent<IGridPointer>().DeActivePointer(graphSelector);
        }


        public override void Destroy() {
        }

        protected override void CommitInternal() {
            graphSelector.CurrentSelected.ExecutableMenu.Commit();
        }

        public override void CommitProgress(int progress) {
            graphSelector?.CurrentSelected.ExecutableMenu.CommitProgress(progress);
        }

        public override void CommitProgressCompleted() {
            graphSelector?.CurrentSelected.ExecutableMenu.CommitProgressCompleted();
        }

        public override void CommitProgressAborted() {
            graphSelector?.CurrentSelected.ExecutableMenu.CommitProgressAborted();
        }

        protected override void AbortInternal() {
            graphSelector.CurrentSelected.ExecutableMenu.Abort();
        }

        public override void InputRaw(Vector2 input) {
            graphSelector.Select(input);
        }
    }
}