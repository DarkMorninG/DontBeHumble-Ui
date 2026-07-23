using DBH.Base;
using UnityEngine;
using UnityEngine.UI;

namespace DBH.UI.Menu.Highlighting {
    public class ColorHighlighter : DBHMono, IHighlighter {
        [SerializeField]
        private RawImage rawImage;

        [SerializeField]
        private Color toChange;

        [SerializeField]
        private Color defaultColor;
                
        
        public void EnableHighlight() {
            rawImage.color = toChange;    
        }

        public void DisableHighlight() {
            rawImage.color = defaultColor;            
        }
    }
}