using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;
using UnityEngine.InputSystem;
using Cinemachine;

public class CannonAimState : PlayerState
{
    private GameObject cannon;
    private GameObject player;
    private CinemachineVirtualCamera camera;
    private Transform cannonBarrel;
    private InputAction moveAction;
    private float rotationSpeed = 30f;
    private bool rotating = false;
    const float minAngle = 15, maxAngle = 65;

    public override void Enter()
    {
        base.Enter();
        EventsPublisher.Instance.PublishEvent("EnterCannonAimState", null, null);
        EventsPublisher.Instance.SubscribeToEvent("EnterCannon", HandleEnterCannon);
        EventsPublisher.Instance.SubscribeToEvent("PlayerMove", HandleCannonMove);
        EventsPublisher.Instance.SubscribeToEvent("PlayerStop", HandleCannonStop);
        EventsPublisher.Instance.SubscribeToEvent("PlayerJump", HandleJump);
        EventsPublisher.Instance.SubscribeToEvent("LookMouse", HandleLook);
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.UnsubscribeToEvent("EnterCannon", HandleEnterCannon);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", HandleCannonMove);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", HandleCannonStop);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerJump", HandleJump);
        EventsPublisher.Instance.UnsubscribeToEvent("LookMouse", HandleLook);
        EventsPublisher.Instance.PublishEvent("ExitCannonAimState", null, null);
    }

    private void HandleEnterCannon(object sender, object data)
    {
        var tuple = ((GameObject, GameObject, CinemachineVirtualCamera))data;
        cannon = tuple.Item1;
        player = tuple.Item2;
        camera = tuple.Item3;
        camera.Priority = 100;
        cannonBarrel = cannon.transform.Find("Barrel");
    }



    private void HandleCannonMove(object sender, object data)
    {
        moveAction = (InputAction)data;
        if (!rotating)
        {
            rotating = true;
            DummyMonoBehavior.Dummy.StartCoroutine(RotateCannon());
        }
    }

    private void HandleCannonStop(object sender, object data)
    {
        rotating = false;
    }

    private IEnumerator RotateCannon()
    {
        while (rotating)
        {
            Vector2 cannonRotation = moveAction.ReadValue<Vector2>();

            cannonBarrel.transform.rotation *= Quaternion.AngleAxis(cannonRotation.x * rotationSpeed * Time.deltaTime, Vector3.up);
            cannonBarrel.transform.rotation *= Quaternion.AngleAxis(-cannonRotation.y * rotationSpeed * Time.deltaTime, Vector3.right);

            var angles = cannonBarrel.transform.localEulerAngles;
            angles.z = 0;
            var angle = angles.x;
            if (angle > 270 && angle < 360 - maxAngle)
            {
                angles.x = 360 - maxAngle;
            }
            else if (angle < 90 && angle > minAngle)
            {
                angles.x = minAngle;
            }

            cannonBarrel.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);

            yield return null;
        }
    }



    private void HandleLook(object sender, object data)
    {
        Vector2 look = (((InputAction, InputAction))data).Item1.ReadValue<Vector2>();

        Transform target = camera.Follow;

        target.transform.rotation *= Quaternion.AngleAxis(look.x, Vector3.up);

        var angles = target.transform.localEulerAngles;
        angles.z = 0;

        target.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
    }



    private void HandleJump(object sender, object data)
    {
        ShootCannon();
        LeaveCannon();
    }

    private void ShootCannon()
    {
        cannonBarrel.Find("Mouth").GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    private void LeaveCannon()
    {
        camera.Priority = 0;
        stateMachine.SwitchState(PlayerStateMachine.DefaultState);
    }

}
