using UnityEngine.InputSystem;

namespace PracticeMonster
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementstateMachine) : base(playerMovementstateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = groundedData.StopData.HardDecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        }
        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

        }
        #endregion

        #region Reusable Methods
        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}
