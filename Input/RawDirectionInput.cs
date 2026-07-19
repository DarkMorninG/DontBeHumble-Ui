using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;

namespace DBH.UI.Input {
    [Bean]
    public class RawDirectionInput : AbstractValueInputSystem<Vector2> {
        protected override string Path => "UI/Navigate";
    }
}