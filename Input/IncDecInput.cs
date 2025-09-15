using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;
using Vault.BetterCoroutine;

namespace DBH.UI.Input {
    [Bean]
    public class IncDecInput : DirectionInputSystem {
        public delegate void IncDecPressed(Direction direction);

        public event IncDecPressed OnIncDecPressed;
        private IAsyncRuntime inputDelay;

        public override string MappedName() {
            return "IncDec";
        }

        protected override void InputChanged(Vector2 input) {
            var inputPressed = input.x > .5f || input.y > .5f || input.x > -.5f || input.y > -.5f;
            if (!inputPressed) return;
            var direction = GetDirection(input);
            if (inputDelay.IsNotRunning()) {
                inputDelay = AsyncRuntime.WaitForSeconds(() => OnIncDecPressed?.Invoke(direction), .2f);
            } else {
                inputDelay.AndAfterFinishDo(() => inputDelay = AsyncRuntime.WaitForSeconds(() => OnIncDecPressed?.Invoke(direction), .2f));
            }
        }


        private Direction GetDirection(Vector2 input) {
            return input.y switch {
                > 0.5f => Direction.VerticalInc,
                < -0.5f => Direction.VerticalDec,
                _ => input.x switch {
                    > 0.5f => Direction.HorizontalInc,
                    < -0.5f => Direction.HorizontalDec,
                    _ => Direction.VerticalInc
                }
            };
        }
    }

    public enum Direction {
        VerticalInc,
        VerticalDec,
        HorizontalDec,
        HorizontalInc
    }
}