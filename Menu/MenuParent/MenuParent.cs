using System;
using System.Collections.Generic;
using System.Linq;
using BetterCoroutine;
using BetterCoroutine.AwaitRuntime;
using DBH.Base;
using DBH.UI.Controller;
using UnityEngine;
using Vault;
using Vault.BetterCoroutine;

namespace DBH.UI.Menu.MenuParent {
    public abstract class MenuParent : DBHMono {
        [SerializeField]
        private DefaultExtensions defaultExtensions;

        private Action onClose;

        public DefaultExtensions DefaultExtensions => defaultExtensions;

        public abstract IExecutableMenu CurrentMenu { get; }

        public abstract void ActivateMenu();
        public abstract void DeActivateMenu();
        public abstract void Destroy();

        protected abstract void CommitInternal(AudioPlayerDto audioPlayerDto);

        public abstract int MenuPointCount();

        public abstract void CommitProgress(int progress);

        public abstract void CommitProgressCompleted();

        public abstract void CommitProgressAborted();

        protected abstract void AbortInternal(AudioPlayerDto audioPlayerDto);

        public void Commit(AudioPlayerDto audioPlayerDto) {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.Commit(audioPlayerDto);
            CommitInternal(audioPlayerDto);
        }

        public void Abort(AudioPlayerDto audioPlayerDto) {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.Abort(audioPlayerDto);
            AbortInternal(audioPlayerDto);
        }

        public bool IncreaseVertical() {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.DirectionInput(ExecutableMenu.Direction.Up);
            return IncreaseVerticalInternal();
        }

        public bool DecreaseVertical() {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.DirectionInput(ExecutableMenu.Direction.Down);
            return DecreaseVerticalInternal();
        }

        public bool IncreaseHorizontal() {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.DirectionInput(ExecutableMenu.Direction.Right);
            return IncreaseHorizontalInternal();
        }

        public bool DecreaseHorizontal() {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            defaultExtensions?.DirectionInput(ExecutableMenu.Direction.Left);
            return DecreaseHorizontalInternal();
        }


        protected virtual bool IncreaseVerticalInternal() {
            return false;
        }

        protected virtual bool DecreaseVerticalInternal() {
            return false;
        }

        protected virtual bool IncreaseHorizontalInternal() {
            return false;
        }

        protected virtual bool DecreaseHorizontalInternal() {
            return false;
        }

        public virtual void InputRaw(Vector2 input) {
        }

        public virtual void CoverAll() {
        }

        public virtual void UnCoverAll() {
        }

        public virtual void Open() {
        }

        protected virtual void CloseInternal() {
        }

        public void Close() {
            onClose?.Invoke();
            CloseInternal();
        }

        public void OnClose(Action callback) {
            onClose = callback;
        }

        public virtual void Reload(Action onFinished = null) {
        }

        protected void MenuButtonSelected() {
            if (defaultExtensions != null) {
                defaultExtensions.ItemHolderOverride = CurrentMenu.Items;
            }

            DefaultExtensions?.DeSelected();
            DefaultExtensions?.Selected();
        }

        protected void CreateMenuPoints(List<GameObject> allMenuPoints, Action<List<MenuPoint>> onFinished) {
            List<MenuPoint> createdMenuPoints = new();
            List<IAwaitRuntime> allRunTimes = new();

            allMenuPoints.ForEach(m => {
                MenuPoint menuPoint;
                var customButton = m.GetComponent<ICustomButton>();
                if (customButton == null) return;
                allRunTimes.Add(IAwaitRuntime.WaitUntil(() => customButton.StartFinished,
                    () => {
                        menuPoint = customButton.Cover.CoverEnabled
                            ? new MenuPoint(m, false, customButton)
                            : new MenuPoint(m, true, customButton);
                        createdMenuPoints.Add(menuPoint);
                    }));
            });
            IAwaitRuntime.WaitUntil(() => allRunTimes.All(runtime => runtime.IsFinished),
                () => {
                    if (!createdMenuPoints.IsEmpty()) {
                        onFinished?.Invoke(createdMenuPoints);
                    }
                });
        }
    }
}