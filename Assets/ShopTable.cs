using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTable : MonoBehaviour
{

    private bool isInRange = false;
    private GameInput inputs;

    public void Awake()
    {
        inputs = new GameInput();
        inputs.Player.Collect.performed += Collect_performed;
        ShopManager.Instance.getTable(transform);
    }
    private void OnEnable()
    {
        inputs.Player.Collect.Enable();
    }
    private void OnDisable()
    {
        inputs.Player.Collect.Disable();
    }
    /*following three functions is to open inventory*/
    private void Collect_performed(InputAction.CallbackContext context)
    {
        if (isInRange )
        {
            ShopManager.Instance.Active();
            ShopManager.Instance.StopHints();
        }

    }
    void OnTriggerEnter(Collider col)
    {
        isInRange = true;
        ShopManager.Instance.ActiveHints();

    }

    void OnTriggerExit(Collider col)
    {
        isInRange = false;
        ShopManager.Instance.Stop();
        ShopManager.Instance.StopHints();
    }
}
