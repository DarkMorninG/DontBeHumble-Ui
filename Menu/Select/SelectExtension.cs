using DBH.Base;
using UnityEngine;

namespace DBH.UI.Menu.Select {
    [RequireComponent(typeof(ExecutableMenu))]
    public abstract class SelectExtension : DBHMono {
        private IExecutableMenu _menuContainer;
        
        public override void OnStart() {
            _menuContainer = GetComponent<IExecutableMenu>();
            _menuContainer.OnSelected += OnSelect;
            _menuContainer.OnDeSelected += OnDeselect;
            _menuContainer.OnSelectedWithItemHolder += OnSelect;
            _menuContainer.OnDeSelectedWithItemHolder += OnDeselect;
        }
        
        protected void OnDestroy() {
            if (_menuContainer == null) return;
            _menuContainer.OnSelected -= OnSelect;
            _menuContainer.OnDeSelected -= OnDeselect;
            _menuContainer.OnSelectedWithItemHolder -= OnSelect;
            _menuContainer.OnDeSelectedWithItemHolder -= OnDeselect;
        }
        
        public virtual void OnSelect() {
        }
        
        public virtual void OnDeselect() {
        }
        
        public virtual void OnSelect(ItemHolder itemHolder) {
        }
        
        public virtual void OnDeselect(ItemHolder itemHolder) {
        }
    }
}