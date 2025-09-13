using DBH.Attributes;
using DBH.Input.api.Extending;
using DBH.UI.Controller;

namespace DBH.UI.Input {
    [Bean]
    public class EnableInputGroup : IMenuInteractionChanged{
        private readonly IInputController inputController;

        public EnableInputGroup(IInputController inputController) {
            this.inputController = inputController;
        }

        public int Order() {
            return 0;
        }

        public void Enabled() {
            inputController.EnableGroup("UI");
        }

        public void Disabled() {
            inputController.DisableGroup("UI");
        }
    }
}