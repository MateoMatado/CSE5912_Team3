using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestAnimation : MonoBehaviour
{
    public GameObject chest;
    private bool isInRange = false;
    private GameInput inputs;
    // Start is called before the first frame update
    public void Awake()
    {
        inputs = new GameInput();
        inputs.Player.Collect.performed += Collect_performed;
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
        if (isInRange && HUDManager.Instance.AbleToBuy(0))
        {
            PickUp();
        }

    }
    void PickUp()
    {
        Destroy(gameObject);
        HUDManager.Instance.CloseCollectPanel();
        Instantiate(chest, transform.position, transform.rotation);
    }


    void OnTriggerEnter(Collider col)
    {
        isInRange = true;
        HUDManager.Instance.DisplayCollectPanel("chest", 0);
    }

    void OnTriggerExit(Collider col)
    {
        isInRange = false;
        HUDManager.Instance.CloseCollectPanel();
    }
}
