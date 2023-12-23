using UnityEngine;

namespace MM2
{
    public class PlayAudioSource : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource=  GetComponent<AudioSource>();
        }

        public void Play()
        {
            _audioSource.Play();
        }
    }
}
