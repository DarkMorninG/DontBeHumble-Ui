using System;
using System.Collections;
using BetterCoroutine.AwaitRuntime;
using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;
using Vault.BetterCoroutine;

namespace DBH.UI.Input {
    [Bean]
    public class ConfirmProgressInput : ButtonInputSystem {
        private IAwaitRuntime confirmHold;

        public delegate void ConfirmHoldProgress(int progress);

        public delegate void ConfirmHoldCompleted();

        public delegate void ConfirmHoldAborted();

        public event ConfirmHoldProgress OnConfirmProgress;
        public event ConfirmHoldCompleted OnConfirmCompleted;
        public event ConfirmHoldAborted OnConfirmHoldAborted;

        public override string MappedName() {
            return "ConfirmProgress";
        }

        public override void OnKeyReleased() {
            if (confirmHold.IsRunning()) {
                confirmHold.Stop();
                OnConfirmHoldAborted?.Invoke();
            }
        }

        public override void OnKeyPressed() {
            confirmHold = IAwaitRuntime.MoveToBackground(() => ConfirmHold(i => OnConfirmProgress?.Invoke(i), () => OnConfirmCompleted?.Invoke()));
        }

        private async void ConfirmHold(Action<int> confirmProgress, Action confirmCompleted) {
            try {
                var timer = 0;
                while (timer <= 100) {
                    timer++;
                    confirmProgress?.Invoke(timer);
                    await Awaitable.WaitForSecondsAsync(.01f);
                }

                confirmCompleted?.Invoke();
            }
            catch (System.Exception e) {
                Debug.LogException(e);
            }
        }
    }
}