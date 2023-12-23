using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private SphereCaster _raycaster;
        [SerializeField] private PlayerInput _playerInput;

        private IInteractable _interactable;

        public UnityEvent InteractableFound;
        public UnityEvent InteractableLost;

        private void OnDrawGizmos()
        {
            _raycaster.DrawDebug();
        }

        private void Update()
        {
            if (_interactable != null)
            {
                if (_playerInput.GameplayMap.GetUseState().WasJustPressed)
                {
                    _interactable.Interact();
                }
            }

            if (_raycaster.FindHit(out EnvironmentQueryResult res))
            {
                if (res.Hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    if (_interactable == null || _interactable != interactable)
                    {
                        InteractableFound?.Invoke();
                        _interactable = interactable;
                    }
                }
            }
            else
            { 
                if (_interactable != null)
                {
                    InteractableLost?.Invoke();
                    _interactable = null;
                }
            }
        }
    }
}
