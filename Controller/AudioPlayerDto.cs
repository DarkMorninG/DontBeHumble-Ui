using UnityEngine;

namespace DBH.UI.Controller {
    public class AudioPlayerDto {
        private AudioSource audioSource;
        private bool hasPlayed;

        public AudioPlayerDto(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        public void Play() {
            if (hasPlayed) return;
            audioSource.Play();
            hasPlayed = true;
        }

        public void Play(AudioClip audioClip) {
            if (hasPlayed) return;
            audioSource.PlayOneShot(audioClip);
            hasPlayed = true;
        }
    }
}