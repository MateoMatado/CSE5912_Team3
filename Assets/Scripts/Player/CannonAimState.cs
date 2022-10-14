using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;
using UnityEngine.InputSystem;
using Cinemachine;
using Team3.PlayerMovement;

public class CannonAimState : PlayerState
{
    private GameObject cannon;
    private GameObject player;
    private CinemachineVirtualCamera camera;
    private Transform cannonBarrel;
    private Transform mouth;
    private InputAction moveAction;
    private float rotationSpeed = 30f;
    private bool rotating = false;
    private bool inCannon = false;
    const float minAngle = 15, maxAngle = 65;
    private float force = 40000;

    public override void Enter()
    {
        base.Enter();
        EventsPublisher.Instance.PublishEvent("EnterCannonAimState", null, null);
        EventsPublisher.Instance.SubscribeToEvent("EnterCannon", HandleEnterCannon);
        EventsPublisher.Instance.SubscribeToEvent("PlayerMove", HandleCannonMove);
        EventsPublisher.Instance.SubscribeToEvent("PlayerStop", HandleCannonStop);
        EventsPublisher.Instance.SubscribeToEvent("PlayerJump", HandleJump);
        EventsPublisher.Instance.SubscribeToEvent("LookMouse", HandleLook);
        EventsPublisher.Instance.SubscribeToEvent("PauseUnpause", HandleEscape);
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.UnsubscribeToEvent("EnterCannon", HandleEnterCannon);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", HandleCannonMove);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", HandleCannonStop);
        EventsPublisher.Instance.UnsubscribeToEvent("PlayerJump", HandleJump);
        EventsPublisher.Instance.UnsubscribeToEvent("LookMouse", HandleLook);
        EventsPublisher.Instance.UnsubscribeToEvent("PauseUnpause", HandleEscape);
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
        mouth = cannonBarrel.Find("Mouth");
        inCannon = true;
        DummyMonoBehavior.Dummy.StartCoroutine(HoldPlayer());
    }

    private IEnumerator HoldPlayer()
    {
        while (inCannon)
        {
            player.transform.position = mouth.position;
            yield return null;
        }
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
    }

    private void ShootCannon()
    {
        LeaveCannon();
        cannon.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        mouth.GetChild(0).GetComponent<ParticleSystem>().Play();
        player.GetComponent<MoveWithCamera>().StartFlying();
        player.transform.position = mouth.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().AddForce(cannonBarrel.forward * force);
    }



    private void HandleEscape(object sender, object data)
    {
        LeaveCannon();
    }

    private void LeaveCannon()
    {
        inCannon = false;
        camera.Priority = 0;
        stateMachine.SwitchState(PlayerStateMachine.DefaultState);
    }

}
