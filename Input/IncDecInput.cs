using BetterCoroutine;
using BetterCoroutine.AwaitRuntime;
using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;
using Vault.BetterCoroutine;

namespace DBH.UI.Input {
    [Bean]
    public class IncDecInput : DirectionInputSystem {
        public delegate void IncDecPressed(Direction direction);

        public event IncDecPressed OnIncDecPressed;
        private IAwaitRuntime inputDelay;

        public override string MappedName() {
            return "IncDec";
        }

        protected override void InputChanged(Vector2 input) {
            if (input == Vector2.zero) return;
            var inputPressed = input.x > .5f || input.y > .5f || input.x > -.5f || input.y > -.5f;
            if (!inputPressed) return;
            var direction = GetDirection(input);
            if (inputDelay.IsNotRunning()) {
                inputDelay = IAwaitRuntime.WaitForSeconds(() => OnIncDecPressed?.Invoke(direction), .05f);
            } else {
                inputDelay.AndAfterFinishDo(() => inputDelay = IAwaitRuntime.WaitForSeconds(() => OnIncDecPressed?.Invoke(direction), .05f));
            }
        }


        private Direction GetDirection(Vector2 input) {
            return input.y switch {
                > 0.5f => Direction.VerticalInc,
                < -0.5f => Direction.VerticalDec,
                _ => input.x switch {
                    > 0.5f => Direction.HorizontalInc,
                    < -0.5f => Direction.HorizontalDec,
                    _ => default
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