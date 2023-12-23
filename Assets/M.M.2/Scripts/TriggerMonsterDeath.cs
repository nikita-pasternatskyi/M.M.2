using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class TriggerMonsterDeath : MonoBehaviour
    {
        [SerializeField] private TriggerEventChannel _launchScreamerEventChannel;
        [SerializeField] private TriggerEventChannel _monsterTriggeredEventChannel;
        [SerializeField] private TriggerEventChannel _hiddenFromMonsterTriggerEventChannel;
        [SerializeField] private float _timeTillDeath;
        [SerializeField] private float _deathTimer;
        [SerializeField] private TextMeshProUGUI _timerPrompt;
        [SerializeField] private string _promptText;
        public UnityEvent Hidden;
        public UnityEvent StartedWarning;
        private bool _running;


        private void OnEnable()
        {
            _monsterTriggeredEventChannel.Triggered += OnTriggered;
            _hiddenFromMonsterTriggerEventChannel.Triggered += OnHidden;
        }

        private void OnDisable()
        {
            _monsterTriggeredEventChannel.Triggered -= OnTriggered;
            _hiddenFromMonsterTriggerEventChannel.Triggered -= OnHidden;
        }

        private void Update()
        {
            if (_running)
            {
                _timerPrompt.text = $"{_promptText} {(int)_timeTillDeath - (int)_deathTimer}";
                _deathTimer += Time.deltaTime;
                if (_deathTimer > _timeTillDeath)
                {
                    _launchScreamerEventChannel.Trigger();
                    _running = false;
                }
            }
        }

        private void OnHidden()
        {
            if (!_running)
                return;
            Hidden?.Invoke();
            _deathTimer = 0;
            _running = false;
        }

        private void OnTriggered()
        {
            StartedWarning?.Invoke();
            _running = true;
        }
    }
}
