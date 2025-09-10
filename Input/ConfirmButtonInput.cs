using DBH.Attributes;
using DBH.Input.api.Extending;

namespace DBH.UI.Input {
    [Bean]
    public class ConfirmButtonInput : ButtonInputSystem {
        public delegate void ConfirmButtonPressed();

        public event ConfirmButtonPressed OnConfirmButtonPressed;

        public override string MappedName() {
            return "Confirm";
        }

        public override void OnKeyReleased() {
            OnConfirmButtonPressed?.Invoke();
        }
    }
}