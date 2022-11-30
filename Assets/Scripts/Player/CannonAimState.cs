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
    const float minAngle = 5, maxAngle = 80;
    private float force = 70000;
    private LineRenderer trajectory;
    private GameObject island = null;
    private Material outlineMaterial;
    private float scrollSpeed = 1f;
    private float minDistance = 20, maxDistance = 100;

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
        EventsPublisher.Instance.SubscribeToEvent("Scroll", HandleScroll);
        if (outlineMaterial == null) outlineMaterial = Resources.Load("IslandOutline") as Material;
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



    private void HandleScroll(object sender, object data)
    {
        float value = -((InputAction)data).ReadValue<Vector2>().normalized.y;
        float current = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        current += value * scrollSpeed;
        current = Mathf.Clamp(current, minDistance, maxDistance);
        camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = current;
    }

    private void HandleEnterCannon(object sender, object data)
    {
        EventsPublisher.Instance.PublishEvent("TurnAnimated", null, null);
        var tuple = ((GameObject, GameObject, CinemachineVirtualCamera))data;
        cannon = tuple.Item1;
        player = tuple.Item2;
        camera = tuple.Item3;
        camera.Priority = 100;
        cannonBarrel = cannon.transform.Find("Barrel");
        mouth = cannonBarrel.Find("Mouth");
        trajectory = mouth.Find("Line").GetComponent<LineRenderer>();
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

            UpdateTrajectory();

            yield return null;
        }
    }

    private void UpdateTrajectory()
    {
        trajectory.positionCount = 300;
        Vector3 startPosition = mouth.position;
        List<Vector3> points = new List<Vector3>() {
            startPosition
        };
        float playerMass = player.GetComponent<Rigidbody>().mass;
        Vector3 initialVelocity = cannonBarrel.forward * force * Time.fixedDeltaTime / playerMass;
        Vector3 prevPoint = startPosition;
        bool hitSomething = false;
        bool hitSurface = false;

        for (int i = 1; i < 300; i++)
        {
            float time = i * .1f;
            Vector3 point = startPosition + time * initialVelocity + .5f * Physics.gravity * time * time;
            points.Add(point);

            int layerMask = 1 << 3;
            layerMask = ~layerMask;
            RaycastHit hit;
            if (Physics.Raycast(prevPoint, (point - prevPoint).normalized, out hit, Vector3.Distance(prevPoint, point), layerMask))
            {
                if (hit.collider.gameObject.layer != 6)
                {
                    GameObject parentIsland = GetParentIsland(hit.collider.transform);
                    if (parentIsland != null)
                    {
                        if (parentIsland != island)
                        {
                            UpdateHitIsland(parentIsland);
                        }

                        hitSomething = true;
                        hitSurface = hitSurface || hit.collider.gameObject.tag == "Surface";
                        trajectory.positionCount = i + 1;

                        if (hitSurface) break;
                    }
                }
            }

            prevPoint = point;
        }
        if (hitSurface)
        {
            Color green = new Color(0, 1, 0, .5f);
            trajectory.startColor = green;
            trajectory.endColor = green;
        }
        else if (hitSomething)
        {
            Color yellow = new Color(1, 1, 0, .5f);
            trajectory.startColor = yellow;
            trajectory.endColor = yellow;
        }
        else
        {
            if (island != null) UpdateHitIsland(null);
            Color red = new Color(1, 0, 0, .5f);
            trajectory.startColor = red;
            trajectory.endColor = red;
        }
        trajectory.SetPositions(points.ToArray());
    }

    private GameObject GetParentIsland(Transform child)
    {
        Transform parent = child;
        while (parent.tag != "IslandParent" && parent.parent != null)
        {
            parent = parent.parent;
        }
        if (parent.tag != "IslandParent") return null;
        return parent.gameObject;
    }

    private void UpdateHitIsland(GameObject newIsland)
    {
        DisableOutline(island);
        EnableOutline(newIsland);
        island = newIsland;
    }

    private void DisableOutline(GameObject g)
    {
        if (g != null)
        {
            foreach (var meshRenderer in g.GetComponentsInChildren<MeshRenderer>())
            {
                var currentMaterials = meshRenderer.materials;
                List<Material> newMaterials = new List<Material>();
                foreach (Material material in currentMaterials)
                {
                    if (!material.name.Contains(outlineMaterial.name))
                    {
                        newMaterials.Add(material);
                    }
                }
                meshRenderer.materials = newMaterials.ToArray();
            }
        }
    }

    private void EnableOutline(GameObject g)
    {
        return;
        if (g != null && g != GetParentIsland(cannon.transform))
        {
            foreach (var meshRenderer in g.GetComponentsInChildren<MeshRenderer>())
            {
                if (meshRenderer.gameObject.name == "ForceField") continue;
                var currentMaterials = meshRenderer.materials;
                List<Material> newMaterials = new List<Material>();
                foreach (Material material in currentMaterials)
                {
                    newMaterials.Add(material);
                }
                newMaterials.Add(outlineMaterial);
                meshRenderer.materials = newMaterials.ToArray();
            }
        }
    }



    private void HandleLook(object sender, object data)
    {
        Vector2 look = (((InputAction, InputAction))data).Item1.ReadValue<Vector2>();

        Transform target = camera.Follow;

        target.transform.rotation *= Quaternion.AngleAxis(look.x, Vector3.up);
        target.transform.rotation *= Quaternion.AngleAxis(-look.y, Vector3.right);

        var angles = target.transform.localEulerAngles;
        angles.z = 0;
        var angle = angles.x;
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 80)
        {
            angles.x = 80;
        }

        target.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
    }



    private void HandleJump(object sender, object data)
    {
        ShootCannon();
    }

    private void ShootCannon()
    {
        EventsPublisher.Instance.PublishEvent("Dissolve", null, GetParentIsland(cannon.transform));
        EventsPublisher.Instance.PublishEvent("Sink", null, GetParentIsland(cannon.transform));
        EventsPublisher.Instance.PublishEvent("LaunchedCannon", null, null);
        LeaveCannon();
        cannon.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        cannon.GetComponent<AudioSource>().Play();
        mouth.Find("CannonShot").GetComponent<ParticleSystem>().Play();
        player.GetComponent<MoveWithCamera>().StartFlying();
        player.transform.position = mouth.position;
        EventsPublisher.Instance.PublishEvent("TurnRagdoll", null, null);
        Team3.Ragdoll.RagRoot ragRoot = player.GetComponentInChildren<Team3.Ragdoll.RagRoot>();
        if (ragRoot == null)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().AddForce(cannonBarrel.forward * force);
        }
        else
        {
            //ragRoot.rootBody.velocity = Vector3.zero;
            //ragRoot.rootBody.AddForce(cannonBarrel.forward * force);

            foreach (Rigidbody body in ragRoot.bodies)
            {
                body.velocity = Vector3.zero;
                body.AddForce(cannonBarrel.forward * force);
            }
        }

    }



    private void HandleEscape(object sender, object data)
    {
        LeaveCannon();
    }

    private void LeaveCannon()
    {
        UpdateHitIsland(null);
        inCannon = false;
        camera.Priority = 0;
        stateMachine.SwitchState(PlayerStateMachine.DefaultState);
    }

}
