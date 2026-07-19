using DBH.Attributes;
using DBH.Input.api.Extending;

namespace DBH.UI.Input {
    [Bean]
    public class ConfirmButtonInput : AbstractButtonInputSystem {
        protected override string Path => "UI/Submit";
    }
}