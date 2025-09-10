using DBH.Attributes;
using DBH.Base;
using UnityEngine;
using UnityEngine.UI;

namespace DBH.UI.Menu {
    public class Cover : DBHMono {
        private Image _image;
        
        [SerializeField]
        private bool coverEnabled;
        
        public bool CoverEnabled => coverEnabled;
        
        [PostConstruct]
        public override void OnStart() {
            if (StartFinished) return;
            _image = GetComponent<Image>();
            if (coverEnabled) {
                Activate();
            }
        }

        public void Hide() {
            if (_image != null) _image.enabled = false;
        }

        public void UnHide() {
            if (_image != null) _image.enabled = true;
        }

        public void Activate() {
            if (_image == null) return;
            var color = _image.color;
            color = new Color(color.r, color.g, color.b, .8f);
            _image.color = color;
            coverEnabled = true;
        }

        public void DeActivate() {
            if (_image == null) return;
            var color = _image.color;
            color = new Color(color.r, color.g, color.b, 0f);
            _image.color = color;
            coverEnabled = false;
        }
    }
}