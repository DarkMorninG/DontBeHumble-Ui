using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;

namespace DBH.UI.Input {
    [Bean]
    public class RawDirectionInput : DirectionInputSystem{
        public delegate void DirectionChanged(Vector2 rawInput);
        public event DirectionChanged OnRawInput;
        public override string MappedName() {
            return "RawDirection";
        }

        protected override void InputChanged(Vector2 input) {
            OnRawInput?.Invoke(input);
        }
    }
}