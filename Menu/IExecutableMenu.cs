
namespace DBH.UI.Menu {
    public interface IExecutableMenu {
        delegate void WithItemHolder(ItemHolder itemHolder);

        delegate void WithItemHolderProgress(int progress, ItemHolder itemHolder);

        ItemHolder Items { get; }

        delegate void VoidNoParameter();
        delegate void ConfirmProgress(int progress);
        event VoidNoParameter OnCommitEvent;
        event VoidNoParameter OnAbortEvent;
        event VoidNoParameter OnCommitProgressCompleted;
        event VoidNoParameter OnCommitProgressAborted;
        event VoidNoParameter OnAfterCommitEvent;
        event VoidNoParameter OnAfterAbortEvent;
        event VoidNoParameter OnDeSelected;
        event VoidNoParameter OnSelected;
        event WithItemHolder OnCommitEventWithItemHolder;
        event WithItemHolder OnAbortEventWithItemHolder;
        event WithItemHolder OnCommitProgressCompletedWithItemHolder;
        event WithItemHolder OnCommitProgressAbortedWithItemHolder;
        event WithItemHolder OnAfterCommitEventWithItemHolder;
        event WithItemHolder OnAfterAbortEventWithItemHolder;
        event WithItemHolder OnDeSelectedWithItemHolder;
        event WithItemHolder OnSelectedWithItemHolder;
        event WithItemHolderProgress OnCommitProgressWithItemHolder;
        void Commit();
        void Abort();
        void CommitProgress(int progress);
        void CommitProgressCompleted();
        void CommitProgressAborted();
        void Selected();
        void DeSelected();
        void DirectionInput(ExecutableMenu.Direction direction);
        event ExecutableMenu.DirectionChanged OnDirectionInput;
    }
}