using System.Collections.Generic;
using DBH.UI.Menu.Commit;
using UnityEngine;
using Vault;

namespace DBH.UI.Menu.Abort {
    public class ClearItemMenuPointsOnCommit : CommitExtension {
        private List<GameObject> _panelsToDelete = new();

        public List<GameObject> PanelsToDelete {
            get => _panelsToDelete;
            set => _panelsToDelete = value;
        }

        public override void OnCommit() {
            foreach (var o in _panelsToDelete) {
                o.GetChildren().ForEach(Destroy);
            }
        }
    }
}