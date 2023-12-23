using MykroFramework.Runtime.Objects.Player.Actions;
using MykroFramework.Runtime.Objects.Player.Web;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Conditions
{
    [System.Serializable]
    public class WebShooterCondition : StateTransitionCondition
    {
        private ISwingPointProvider _webShooter;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _webShooter = stateMachine.GetComponent(typeof(ISwingPointProvider)) as ISwingPointProvider;
        }

        public override bool Check()
        {
            return _webShooter.CanSwing;
        }
    }
}