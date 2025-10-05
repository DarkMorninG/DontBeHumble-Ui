using System;
using UnityEngine;

namespace DBH.UI.Controller {
    [Serializable]
    public class AudioSourceDto {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private MenuActionType menuActionType;

        public AudioSource AudioSource => audioSource;

        public MenuActionType MenuActionType => menuActionType;
    }
}