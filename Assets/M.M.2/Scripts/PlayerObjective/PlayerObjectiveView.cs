using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MM2
{
    public class PlayerObjectiveView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _progressBar;
        private int _maxAmount;
        private int _currentAmount;
        public UnityEvent Completed;

        private PlayerObjective _playerObjective;

        public void Init(PlayerObjective objective)
        {
            _playerObjective = objective;
            _title.text = _playerObjective.Title;
            _description.text = _playerObjective.Description;
            _maxAmount = objective.AmountForCompletion;
            objective.AmountAdded += AddAmount;
        }

        private void OnDisable()
        {
            _playerObjective.AmountAdded -= AddAmount;
        }

        public void AddAmount()
        {
            if(_currentAmount <= _maxAmount)
                _currentAmount++;
            _progressBar.fillAmount = (float)_currentAmount / (float)_maxAmount;
            if (_currentAmount == _maxAmount)
            {
                Complete();
            }

        }

        public void Complete()
        {
            Completed?.Invoke();
        }
    }
}
