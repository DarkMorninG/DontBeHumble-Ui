using DBH.Base;

namespace DBH.UI.Menu.Commit {
    public abstract class CommitExtension : DBHMono {
        private ExecutableMenu _menuContainer;

        private void Start() {
            _menuContainer = GetComponent<ExecutableMenu>();
            _menuContainer.OnCommitEvent += OnCommit;
            _menuContainer.OnCommitEventWithItemHolder += OnCommit;
            _menuContainer.OnCommitProgressCompleted += OnCommitProgressFinished;
            _menuContainer.OnCommitProgressCompletedWithItemHolder += OnCommitProgressFinished;
            _menuContainer.OnCommitProgressAborted += OnCommitProgressAborted;
            _menuContainer.OnCommitProgressAbortedWithItemHolder += OnCommitProgressAborted;
            _menuContainer.OnAfterCommitEvent += AfterOnCommit;
            _menuContainer.OnAfterCommitEventWithItemHolder += AfterOnCommit;
            _menuContainer.OnCommitProgress += OnCommitProgress;
            _menuContainer.OnCommitProgressWithItemHolder += OnCommitProgress;
        }

        protected void OnDestroy() {
            if (_menuContainer == null) return;
            _menuContainer.OnCommitEvent -= OnCommit;
            _menuContainer.OnCommitEventWithItemHolder -= OnCommit;
            _menuContainer.OnCommitProgressCompleted -= OnCommitProgressFinished;
            _menuContainer.OnCommitProgressCompletedWithItemHolder -= OnCommitProgressFinished;
            _menuContainer.OnCommitProgressAborted -= OnCommitProgressAborted;
            _menuContainer.OnCommitProgressAbortedWithItemHolder -= OnCommitProgressAborted;
            _menuContainer.OnAfterCommitEventWithItemHolder -= AfterOnCommit;
            _menuContainer.OnAfterCommitEvent -= AfterOnCommit;
            _menuContainer.OnCommitProgress -= OnCommitProgress;
            _menuContainer.OnCommitProgressWithItemHolder -= OnCommitProgress;
        }

        public virtual void OnCommit() {
        }

        public virtual void OnCommit(ItemHolder itemHolder) {
        }

        public virtual void OnCommitProgress(int progress) {
        }

        public virtual void OnCommitProgress(int progress, ItemHolder itemHolder) {
        }

        public virtual void OnCommitProgressFinished() {
        }

        public virtual void OnCommitProgressFinished(ItemHolder itemHolder) {
        }

        public virtual void OnCommitProgressAborted() {
        }

        public virtual void OnCommitProgressAborted(ItemHolder itemHolder) {
        }

        public virtual void AfterOnCommit() {
        }

        public virtual void AfterOnCommit(ItemHolder itemHolder) {
        }
    }
}