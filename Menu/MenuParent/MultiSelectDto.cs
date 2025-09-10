using System;
using UnityEngine;

namespace DBH.UI.Menu.MenuParent {
    public class MultiSelectDto {
        private ICustomButton _customButton;
        private IExecutableMenu _executableMenu;
        private GameObject _gameObject;

        public MultiSelectDto(ICustomButton customButton, IExecutableMenu executableMenu, GameObject gameObject) {
            _customButton = customButton;
            _executableMenu = executableMenu;
            _gameObject = gameObject;
        }

        public ICustomButton CustomButton => _customButton;
        public IExecutableMenu ExecutableMenu => _executableMenu;
        public GameObject GameObject => _gameObject;

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is not MultiSelectDto dto) return false;
            return dto._customButton.Equals(_customButton) && dto._executableMenu.Equals(_executableMenu) && dto._gameObject.Equals(_gameObject);
        }
        
        public override int GetHashCode() {
            return HashCode.Combine(_customButton, _executableMenu, _gameObject);
        }
    }
}