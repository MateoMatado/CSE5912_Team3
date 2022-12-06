using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Player;
using Team3.Events;
using Cinemachine;

public class CannonCollision : MonoBehaviour
{
    [SerializeField] private int playerLayer = 3;
    [SerializeField] private CinemachineVirtualCamera cannonCamera;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == playerLayer)
        {
            PlayerStateManager stateManager = collider.GetComponent<PlayerStateManager>();
            if (stateManager != null && stateManager.StateMachine.SwitchState(PlayerStateMachine.CannonAimState))
            {
                EventsPublisher.Instance.PublishEvent("EnterCannon", this, (
                    gameObject, collider.gameObject.GetComponent<Team3.Scripts.Player.PlayerSwap>().current, cannonCamera
                ));
            }
        }
    }
}
