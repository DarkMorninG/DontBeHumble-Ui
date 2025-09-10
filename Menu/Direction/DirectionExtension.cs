using DBH.Base;
using UnityEngine;

namespace DBH.UI.Menu.Direction {
    [RequireComponent(typeof(ExecutableMenu))]
    public abstract class DirectionExtension : DBHMono {
        private IExecutableMenu _menuContainer;
        protected ItemHolder ItemHolder;


        public override void OnStart() {
            _menuContainer = GetComponent<IExecutableMenu>();
            ItemHolder = _menuContainer.Items;
            _menuContainer.OnSelectedWithItemHolder += OnSelect;
            _menuContainer.OnDirectionInput += OnDirection;
            _menuContainer.OnDeSelected += OnDeselect;
        }

        protected virtual void OnDirection(ExecutableMenu.Direction direction) {
        }

        protected virtual void OnSelect(ItemHolder itemHolder) {
        }

        protected virtual void OnDeselect() {
            
        }
    }
}