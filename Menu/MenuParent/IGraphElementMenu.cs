using System.Collections.Generic;
using UnityEngine;

namespace DBH.UI.Menu.MenuParent {
    public interface IGraphElementMenu {
        Dictionary<GameObject, Vector2> GetDirections();
    }
}