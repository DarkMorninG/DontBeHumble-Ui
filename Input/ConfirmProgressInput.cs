using System;
using System.Collections;
using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;
using Vault.BetterCoroutine;

namespace DBH.UI.Input {
    [Bean]
    public class ConfirmProgressInput : ButtonInputSystem{
        private IAsyncRuntime confirmHold;
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
            confirmHold = UnityAsyncRuntime.Create(ConfirmHold(i => OnConfirmProgress?.Invoke(i), () => OnConfirmCompleted?.Invoke()));
        }

        private IEnumerator ConfirmHold(Action<int> confirmProgress, Action confirmCompleted) {
            var timer = 0;
            while (timer <= 100) {
                timer++;
                confirmProgress?.Invoke(timer);
                yield return new WaitForSecondsRealtime(.01f);
            }

            confirmCompleted?.Invoke();
        }
    }
}