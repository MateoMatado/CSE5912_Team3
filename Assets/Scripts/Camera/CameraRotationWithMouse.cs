using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Team3.Player;

public class CameraRotationWithMouse : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float rotationSpeed;
    private Vector2 look = Vector2.zero;
    private PlayerStateManager stateManager;

    void Awake()
    {
        stateManager = GetComponent<PlayerStateManager>();
        Team3.Events.EventsPublisher.Instance.SubscribeToEvent("Look", Look);
    }

    private void OnDestroy()
    {
        Team3.Events.EventsPublisher.Instance.UnsubscribeToEvent("Look", Look);
    }

    private void Look(object sender, object data)
    {
        if (stateManager.StateMachine.CurrentState is TargetingState) return;
        if (stateManager.StateMachine.CurrentState is IKState) return;

        look = ((InputAction)data).ReadValue<Vector2>();

        cameraTarget.transform.rotation *= Quaternion.AngleAxis(look.x * rotationSpeed, Vector3.up);
        cameraTarget.transform.rotation *= Quaternion.AngleAxis(-look.y * rotationSpeed, Vector3.right);
        
        var angles = cameraTarget.transform.localEulerAngles;
        angles.z = 0;
        var angle = angles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        // transform.rotation = Quaternion.Euler(0, cameraTarget.transform.rotation.eulerAngles.y, 0);
        cameraTarget.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
    }
}
