using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;


public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public List<Item> Equipment = new List<Item>();
    public List<Item> Equipped = new List<Item>();
    private GameInput inputs;
    public Transform ItemContent;
    public static bool Opened = false;
    public GameObject EquipmentInventory;
    public GameObject Inventoty;
    public Transform Player;
    public List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> Equipinventory = new List<GameObject>();
    private Item tempEquipment;



    // Normal Set up
    public void Awake()
    {
        Instance = this;
        inputs = new GameInput();
        inputs.Player.EquipmentInventory.performed += EquipmentInventory_performed;
        inputs.Player.EquipmentInventory.canceled += EquipmentInventory_performed;
    }
    private void OnEnable()
    {
        inputs.Player.EquipmentInventory.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.EquipmentInventory.Disable();
    }

    /*following three functions is to open inventory*/
    private void EquipmentInventory_performed(InputAction.CallbackContext context)
    {
        if (!Opened)
        {
            Open();
            ListItems();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        Inventoty.SetActive(true);
        EquipmentInventory.SetActive(true);
        Opened = true;
    }

    public void Close()
    {
        Inventoty.SetActive(false);
        Opened = false;
    }

    /*to get item and drop item*/
    public void Add(Item item)
    {
        Equipment.Add(item);
        ListItems();
    }

    public void Equip(Text name)
    {
        for(int i = 0; i < Equipment.Count; i++)
        {
            Item item = Equipment[i];
            if (item.itemName.Equals(name.text))
            {
                Equipment.Remove(item);
                Equipped.Add(item);
            }
        }
        ListItems();
    }

    public void Drop(Text name)
    {
        for (int i = 0; i < Equipment.Count; i++)
        {
            Item item = Equipment[i];
            if (item.itemName.Equals(name.text))
            {
                Equipment.Remove(item);
                GameObject obj = ToolsFactory.Instance.GetDropObject(name);
                Vector3 newPosition = Player.position + (Player.forward * 8) + (Player.up );
                Instantiate(obj, newPosition, Player.rotation);
            }
        }
        ListItems();
    }

    /*update the item in inventory*/
    public void ListItems()
    {
        for (int i = 0; i< inventory.Count; i++)
        {            
            GameObject obj = inventory[i];
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            if(i< Equipment.Count)
            {
                Item item = Equipment[i];
                itemIcon.color = new Color32(255, 255, 255, 255);
                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
            }
            else
            {
                itemName.text = "";
                itemIcon.sprite = null;
                itemIcon.color = new Color32(255, 255, 255, 0);
            }

        }

        for (int i = 0; i < Equipped.Count; i++)
        {
            Item item = Equipped[i];
            GameObject obj = Equipinventory[i];
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            itemIcon.color = new Color32(255, 255, 255, 255);
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }

    }


}
