using UnityEngine;

namespace DBH.UI.Menu.MenuParent {
    public class MenuPoint {
        private GameObject target;

        private bool selectable;

        private ICustomButton button;


        public MenuPoint(GameObject target, bool selectable, ICustomButton customButton) {
            this.target = target;
            this.selectable = selectable;
            button = customButton;
        }

        public GameObject Target => target;

        public bool Selectable => selectable;

        public ICustomButton Button => button;
    }
}