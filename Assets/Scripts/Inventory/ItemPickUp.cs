using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ItemPickUp : MonoBehaviour
{
    public Item Item;
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
        if (isInRange && HUDManager.Instance.AbleToBuy(Item.Value))
        {
            PickUp();
        }

    }
    void PickUp()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        HUDManager.Instance.GetCoin(Item.Value*-1);
        HUDManager.Instance.CloseCollectPanel();
    }

    void OnTriggerEnter(Collider col)
    {
        isInRange = true;
        HUDManager.Instance.DisplayCollectPanel(Item.itemName, Item.Value);
    }

    void OnTriggerExit(Collider col)
    {
        isInRange = false;
        HUDManager.Instance.CloseCollectPanel();
    }

}
