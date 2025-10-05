using UnityEngine;

namespace DBH.UI.Controller {
    public class AudioPlayerDto {
        private AudioSource audioSource;

        public AudioPlayerDto(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        public void Play() {
            audioSource.Play();
        }

        public void Play(AudioClip audioClip) {
            audioSource.PlayOneShot(audioClip);
        }
    }
}