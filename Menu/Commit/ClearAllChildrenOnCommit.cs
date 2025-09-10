using UnityEngine;
using Vault;

namespace DBH.UI.Menu.Commit {
    public class ClearAllChildrenOnCommit : CommitExtension {
        [SerializeField]
        private GameObject parent;


        public override void OnCommit() {
            parent.GetChildren().ForEach(Destroy);
        }
    }
}