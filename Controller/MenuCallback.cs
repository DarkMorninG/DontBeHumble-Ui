using System;

namespace DBH.UI.Controller {
    public class MenuCallback {
        private Action onCloseAction;

        public void OnMenuClose(Action callback) {
            onCloseAction = callback;
        }

        public void CallClose() {
            onCloseAction?.Invoke();
        }
    }
}