using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player
{
    public class WebZipRules : MonoBehaviour
    {
        [SerializeField] private SOStateMachine.SOStateMachine _stateMachine;
        [SerializeField] private List<SOStateMachine.StateContainer> _statesThatDoNotResetZipCount;
        [SerializeField] private StateContainer _webZipState;
        [SerializeField] private int _webZipMaxCount;
        [SerializeField] private AnimationCurve _multiplierCurve;
        [SerializeField] private ConeCaster _coneScanner;
        private int _webZipCount;
        [ReadOnly] public float WebZipMultiplier = 1;
        [ReadOnly] public Vector3 LastZipPoint;

        private void OnDrawGizmos()
        {
            _coneScanner.DrawDebug();
        }

        private void OnEnable()
        {
            _coneScanner.Init();
            WebZipMultiplier = 1;
            _stateMachine.NewStateEntered += OnStateEntered;
        }

        private void OnStateEntered(SOStateMachine.StateContainer obj)
        {
            if (obj == _webZipState)
            {
                _webZipCount = Mathf.Clamp(_webZipCount + 1, 0, _webZipMaxCount);
                UpdateZipMultiplier();
                return;
            }

            bool update = true;
            foreach (var state in _statesThatDoNotResetZipCount)
            {
                if (state == obj)
                {
                    update = false;
                    break;
                }
            }
            if (!update)
                return;
            _webZipCount = 0;
            UpdateZipMultiplier();
        }

        private void OnDisable()
        {
            _stateMachine.NewStateEntered -= OnStateEntered;
        }

        private void UpdateZipMultiplier()
        {
            WebZipMultiplier = _multiplierCurve.Evaluate(1f - ((float)_webZipCount / _webZipMaxCount));
        }

        public bool CanWebZip(out RaycastHit hit, out Vector3 dir)
        {
            hit = default;
            dir = default;
            if (_webZipCount > _webZipMaxCount)
                return false;
            if (_coneScanner.FindHit(out EnvironmentQueryResult environmentQueryResult))
            {
                LastZipPoint = environmentQueryResult.Hit.point;
                return true;
            }
            return false;
        }
    }
}