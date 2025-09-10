using UnityEngine;

namespace DBH.UI.Menu.Commit {
    public class EnableGameobjectOnCommit : CommitExtension {
        [SerializeField]
        private GameObject toEnable;

        public override void OnCommit(ItemHolder itemHolder) {
            if (toEnable == null) {
                itemHolder.Item<GameObject>().SetActive(true);
            } else {
                toEnable.SetActive(true);
            }
        }
    }
}