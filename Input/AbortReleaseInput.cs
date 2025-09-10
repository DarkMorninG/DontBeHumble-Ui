using DBH.Attributes;
using DBH.Input.api.Extending;

namespace DBH.UI.Input {
    [Bean]
    public class AbortInput : ButtonInputSystem {
        public delegate void AbortPressed();
        public event AbortPressed OnAbortPressed;
        public override string MappedName() {
            return "Abort";
        }

        public override void OnKeyReleased() {
            OnAbortPressed?.Invoke();
        }
    }
}