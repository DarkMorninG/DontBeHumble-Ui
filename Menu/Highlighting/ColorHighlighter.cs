using DBH.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DBH.UI.Menu.Highlighting {
    public class ColorHighlighter : DBHMono, IHighlighter {
        [SerializeField]
        [FormerlySerializedAs("rawImage")]
        [Tooltip("The UI Image or Raw Image whose color should be highlighted.")]
        private Graphic image;

        [SerializeField]
        private Color toChange;

        [SerializeField]
        private Color defaultColor;
                
        
        public void EnableHighlight() {
            image.color = toChange;    
        }

        public void DisableHighlight() {
            image.color = defaultColor;            
        }
    }
}
