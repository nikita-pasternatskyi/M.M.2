using UnityEngine;

namespace MM2
{
    public class GameOverRules : MonoBehaviour
    {
        [SerializeField] private TriggerEventChannel _gameOverEventChannel;
        [SerializeField] private Monster _monster;

        private void OnEnable()
        {
            _gameOverEventChannel.Triggered += OnGameOverAsked;
        }

        private void OnDisable()
        {
            _gameOverEventChannel.Triggered -= OnGameOverAsked;
        }

        private void OnGameOverAsked()
        {

        }
    }
}
