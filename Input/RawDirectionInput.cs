using DBH.Attributes;
using DBH.Input.api.Extending;
using UnityEngine;

namespace DBH.UI.Input {
    [Bean]
    public class RawDirectionInput : AbstractValueInputSystem<Vector2> {
        public override string Path => "UI/Navigate";
    }
}