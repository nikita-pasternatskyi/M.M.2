using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine.Conditions
{
    [System.Serializable]
    public class SetTimeScale : StateAction
    {
        [SerializeField] private GameTime _gameTime;
        [SerializeField] private float _targetTimeScale;

        public override void Enter()
        {
            _gameTime.ChangeTimeScale(_targetTimeScale);
        }

        public override void Exit()
        {
            _gameTime.RestoreTimeScale();
        }
    }
}

