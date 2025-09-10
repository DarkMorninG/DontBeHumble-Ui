using DBH.Base;

namespace DBH.UI.Menu.Abort {
    public abstract class AbortExtension : DBHMono {
        private ExecutableMenu _menuContainer;
        
        private void Start() {
            _menuContainer = GetComponent<ExecutableMenu>();
            _menuContainer.OnAbortEvent += OnAbort;
            _menuContainer.OnAfterAbortEvent += AfterOnAbort;
            _menuContainer.OnAbortEventWithItemHolder += OnAbort;
            _menuContainer.OnAfterAbortEventWithItemHolder += AfterOnAbort;
        }
        
        private void OnDestroy() {
            if (_menuContainer == null) return;
            _menuContainer.OnAbortEvent -= OnAbort;
            _menuContainer.OnAfterAbortEvent -= AfterOnAbort;
            _menuContainer.OnAbortEventWithItemHolder -= OnAbort;
            _menuContainer.OnAfterAbortEventWithItemHolder -= AfterOnAbort;
        }
        
        public virtual void OnAbort() {
        }
        
        protected void AfterOnAbort() {
        }
        
        public virtual void OnAbort(ItemHolder itemHolder) {
        }
        
        protected void AfterOnAbort(ItemHolder itemHolder) {
        }
    }
}