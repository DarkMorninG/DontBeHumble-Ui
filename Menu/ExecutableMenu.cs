using DBH.Base;
using DBH.UI.Menu.Blocker;
using UnityEngine;

namespace DBH.UI.Menu {
    public abstract class ExecutableMenu : DBHMono, IExecutableMenu, ICustomButton {
        public enum Direction {
            Up,
            Down,
            Left,
            Right
        }

        [SerializeField]
        private ItemHolder itemHolder;

        public delegate void DirectionChanged(Direction direction);

        public abstract Cover Cover { get; }


        public event IExecutableMenu.VoidNoParameter OnCommitEvent;
        public event IExecutableMenu.VoidNoParameter OnCommitProgressCompleted;
        public event IExecutableMenu.VoidNoParameter OnSelected;
        public event IExecutableMenu.VoidNoParameter OnDeSelected;
        public event IExecutableMenu.VoidNoParameter OnCommitProgressAborted;
        public event IExecutableMenu.VoidNoParameter OnAbortEvent;
        public event IExecutableMenu.VoidNoParameter OnAfterCommitEvent;
        public event IExecutableMenu.VoidNoParameter OnAfterAbortEvent;

        public event DirectionChanged OnDirectionInput;
        public event IExecutableMenu.WithItemHolderProgress OnCommitProgressWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnCommitEventWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnAbortEventWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnCommitProgressCompletedWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnCommitProgressAbortedWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnAfterCommitEventWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnAfterAbortEventWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnDeSelectedWithItemHolder;
        public event IExecutableMenu.WithItemHolder OnSelectedWithItemHolder;

        public ItemHolder Items => ItemHolder();

        protected virtual ItemHolder ItemHolder() {
            return itemHolder;
        }

        public void Commit() {
            var commitBlock = GetComponent<ICommitBlock>();
            if (commitBlock != null && commitBlock.Denied()) return;
            OnCommit();
        }

        public void DirectionInput(Direction direction) {
            OnDirectionInput?.Invoke(direction);
        }

        public void CommitProgress(int progress) {
            if (itemHolder != null) {
                OnCommitProgressWithItemHolder?.Invoke(progress, ItemHolder());
            }

            OnCommitProgress?.Invoke(progress);
        }

        public void CommitProgressCompleted() {
            if (itemHolder != null) {
                OnCommitProgressCompletedWithItemHolder?.Invoke(ItemHolder());
            }

            OnCommitProgressCompleted?.Invoke();
        }

        public void CommitProgressAborted() {
            if (itemHolder != null) {
                OnCommitProgressAbortedWithItemHolder?.Invoke(ItemHolder());
            }

            OnCommitProgressAborted?.Invoke();
        }

        public void Selected() {
            if (itemHolder != null) {
                OnSelectedWithItemHolder?.Invoke(ItemHolder());
            }

            OnSelected?.Invoke();
        }

        public void DeSelected() {
            if (itemHolder != null) {
                OnDeSelectedWithItemHolder?.Invoke(ItemHolder());
            }
            OnDeSelected?.Invoke();
        }

        public void Abort() {
            OnAbort();
        }

        public event IExecutableMenu.ConfirmProgress OnCommitProgress;


        private void OnCommit() {
            if (ItemHolder() != null) {
                OnCommitEventWithItemHolder?.Invoke(ItemHolder());
                OnAfterCommitEventWithItemHolder?.Invoke(ItemHolder());
            }

            OnCommitEvent?.Invoke();
            OnAfterCommitEvent?.Invoke();
        }


        private void OnAbort() {
            if (ItemHolder() != null) {
                OnAbortEventWithItemHolder?.Invoke(ItemHolder());
                OnAfterAbortEventWithItemHolder?.Invoke(ItemHolder());
            }

            OnAbortEvent?.Invoke();
            OnAfterAbortEvent?.Invoke();
        }
    }
}