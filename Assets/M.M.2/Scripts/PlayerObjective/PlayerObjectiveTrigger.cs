using UnityEngine;
using UnityEngine.Events;

namespace MM2
{

    public enum ObjectiveTriggerType
    {
        GiveObjective, CompleteObjective
    }

    public class PlayerObjectiveTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerObjective _playerObjective;
        [SerializeField] private ObjectiveTriggerType _type;

        public UnityEvent Triggered;

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.TryGetComponent(out PlayerObjectives playerObjectives))
            {
                if (_type == ObjectiveTriggerType.GiveObjective)
                    playerObjectives.RegisterObjective(_playerObjective);
                else
                    _playerObjective.Complete();
                Triggered?.Invoke();
            }
        }
    }
}
