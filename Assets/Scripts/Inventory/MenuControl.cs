using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuControl : MonoBehaviour
{
    public GameObject EquipmentInventory;
    public GameObject Inventory;
    public GameObject GeneralInventory;
    public static bool check1 = false;  //The general Inventory
    public static bool inventory = false;
    public static bool equipment = false;
    private GameInput inputs;

    public void Awake()
    {
        inputs = new GameInput();
        inputs.Player.Inventory.performed += Inventory_performed;
        inputs.Player.Inventory.canceled += Inventory_performed;
        inputs.Player.EquipmentInventory.performed += EquipmentInventory_performed;
        inputs.Player.EquipmentInventory.canceled += EquipmentInventory_performed;
    }
    private void OnEnable()
    {
        inputs.Player.Inventory.Enable();
        inputs.Player.EquipmentInventory.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Inventory.Disable();
        inputs.Player.EquipmentInventory.Disable();
    }

    /*following three functions is to open inventory*/
    private void Inventory_performed(InputAction.CallbackContext context)
    {
        if (!inventory)
        {
            OpenInventory();
            InventoryManager.Instance.ListItems();
        }
        else
        {
            CloseInventory();
        }
    }

    private void EquipmentInventory_performed(InputAction.CallbackContext context)
    {
        if (!equipment)
        {
            OpenEquipment();
            EquipmentManager.Instance.ListItems();
        }
        else
        {
            CloseEquipment();
        }
    }

    public void OpenInventory()
    {
        GeneralInventory.SetActive(true);
        Inventory.SetActive(true);
        EquipmentInventory.SetActive(false);
        check1 = true;
        inventory = true;
        equipment = false;
    }

    public void CloseInventory()
    {
        if(check1 && !equipment)
        {
            GeneralInventory.SetActive(false);
            Inventory.SetActive(false);
            check1 = false;
            inventory = false;
        }

    }
    public void OpenEquipment()
    {
        GeneralInventory.SetActive(true);
        Inventory.SetActive(false);
        EquipmentInventory.SetActive(true);
        check1 = true;
        equipment = true;
        inventory = false;
    }

    public void CloseEquipment()
    {
        if (check1 && !inventory)
        {
            GeneralInventory.SetActive(false);
            EquipmentInventory.SetActive(false);
            check1 = false;
            equipment = false;
        }
    }

}
