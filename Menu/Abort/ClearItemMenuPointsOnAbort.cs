using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace DBH.UI.Menu.Abort {
    public class ClearItemMenuPointsOnAbort : AbortExtension {
        private List<GameObject> _panelsToDelete = new();

        public List<GameObject> PanelsToDelete {
            get => _panelsToDelete;
            set => _panelsToDelete = value;
        }

        public override void OnAbort() {
            foreach (var o in _panelsToDelete) {
                o.GetChildren().ForEach(Destroy);
            }
        }
    }
}