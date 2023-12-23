using MykroFramework.Runtime.Objects.Player;
using UnityEngine;

namespace MM2
{
    public class PlayerFootSteps : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private GroundedState _state;

        private void Update()
        {
            if (_playerInput.AbsoluteMovementInput != Vector2.zero && !_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            else if(_audioSource.isPlaying && _playerInput.AbsoluteMovementInput == Vector2.zero)
            {
                _audioSource.Stop();
            }
        }
    }
}
