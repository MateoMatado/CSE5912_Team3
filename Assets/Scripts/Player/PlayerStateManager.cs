using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Player 
{
    public class PlayerStateManager : MonoBehaviour
    {
        private PlayerStateMachine stateMachine;
        private bool leftArmActive = false, rightArmActive = false;

        void Awake() 
        {
            stateMachine = new PlayerStateMachine();

            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", (object sender, object data) => { UpdateLeftArm(true); });
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmDeactivate", (object sender, object data) => { UpdateLeftArm(false); });
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", (object sender, object data) => { UpdateRightArm(true); });
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmDeactivate", (object sender, object data) => { UpdateRightArm(false); });
        
            Events.EventsPublisher.Instance.SubscribeToEvent("Target", HandleTargetEvent);
        }

        private void HandleTargetEvent(object sender, object data)
        {
            if (stateMachine.CurrentState is TargetingState)
            {
                stateMachine.SwitchState(PlayerStateMachine.DefaultState);
                UpdateStateWithArms();
            }
            else
            {
                stateMachine.SwitchState(PlayerStateMachine.TargetingState);
            }
        }

        private void UpdateLeftArm(bool active)
        {
            leftArmActive = active;
            UpdateStateWithArms();
        }

        private void UpdateRightArm(bool active)
        {
            rightArmActive = active;
            UpdateStateWithArms();
        }

        private void UpdateStateWithArms()
        {
            if ((leftArmActive || rightArmActive) && stateMachine.CurrentState is DefaultPlayerState)
            {
                stateMachine.SwitchState(PlayerStateMachine.IKState);
            }
            else if ((!leftArmActive && !rightArmActive) && stateMachine.CurrentState is IKState)
            {
                stateMachine.SwitchState(PlayerStateMachine.DefaultState);
            }
        }
    }
}
