using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Conditions
{
    [System.Serializable]
    public class WebZipCondition : StateTransitionCondition
    {
        private WebZipRules _webZipRules;
        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _webZipRules = stateMachine.GetComponent<WebZipRules>();
        }

        public override bool Check()
        {
            return _webZipRules.CanWebZip(out RaycastHit hit, out Vector3 direction);
        }
    }
}