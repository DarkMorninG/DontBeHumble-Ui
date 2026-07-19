using DBH.Attributes;
using DBH.Input.api.Extending;

namespace DBH.UI.Input {
    [Bean]
    public class AbortInput : AbstractButtonInputSystem {
        protected override string Path => "UI/Cancel";
    }
}