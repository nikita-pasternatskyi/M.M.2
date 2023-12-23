using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class LastGenerator : MonoBehaviour, IInteractable
    {
        [SerializeField] private PlayerObjective[] _playerObjectivesToCheck;
        public UnityEvent Interacted;
        private int _completion;
        private int _allCalls;

        private void Awake()
        {
            foreach(var i in _playerObjectivesToCheck) 
            {
                _allCalls += i.AmountForCompletion;
            }
        }

        private void OnEnable()
        {
            foreach (var item in _playerObjectivesToCheck)
            {
                item.AmountAdded += Item_Completed;
            }
        }

        private void Item_Completed()
        {
            _completion++;
        }

        private void OnDisable()
        {
            foreach (var item in _playerObjectivesToCheck)
            {
                item.AmountAdded += Item_Completed;
            }
        }

        public bool Check()
        {
            if (_completion >= _allCalls)
                return true;
            Destroy(this);
            return false;
        }

        public void Interact()
        {
            if (Check())
            {
                Interacted?.Invoke();
            }
        }
        public void LockInteractivity() => Destroy(this);
    }

}
