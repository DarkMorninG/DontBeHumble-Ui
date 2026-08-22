using DBH.UI.Controller;
using UnityEngine;

namespace DBH.UI.Menu.MenuParent {
    public class EmptyMenuParent : MenuParent {
        [SerializeField]
        private ExecutableMenu onlyMenu;


        public override IExecutableMenu CurrentMenu => onlyMenu;

        public override void ActivateMenu() {
        }

        public override void DeActivateMenu() {
        }

        public override void Destroy() {
            
        }

        protected override void CommitInternal(AudioPlayerDto audioPlayerDto) {
            onlyMenu.Commit(audioPlayerDto);
        }

        public override int MenuPointCount() {
            return 1;
        }

        public override void CommitProgress(int progress) {
            onlyMenu.CommitProgress(progress);
        }

        public override void CommitProgressCompleted() {
            onlyMenu.CommitProgressCompleted();
        }

        public override void CommitProgressAborted() {
            onlyMenu.CommitProgressAborted();
        }

        protected override void AbortInternal(AudioPlayerDto audioPlayerDto) {
            onlyMenu.Abort(audioPlayerDto);
        }
    }
}