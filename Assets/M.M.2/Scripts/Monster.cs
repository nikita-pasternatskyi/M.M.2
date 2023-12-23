using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;

namespace MM2
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private TriggerEventChannel _screamerChannel;
        [SerializeField] private GameObject _phase2;
        [SerializeField] private GameObject _phase3;
        public UnityEvent Screaming;
        private Animator _animator;

        private void Awake()
        {
            _animator = _phase2.GetComponent<Animator>();
            _phase2.SetActive(false);
            _phase3.SetActive(false);
        }

        private void OnEnable()
        {
            _screamerChannel.Triggered += OnTriggered;
        }

        private void OnDisable()
        {
            _screamerChannel.Triggered -= OnTriggered;
        }

        private void OnTriggered()
        {
            Scream();
        }

        public void ChangeIntoPhase3()
        {
            _animator = _phase3.GetComponent<Animator>();
        }

        public void Scream()
        {
            Screaming?.Invoke();
            _animator.gameObject.SetActive(true);
            _animator.SetTrigger("Scream");
        }
    }
}
