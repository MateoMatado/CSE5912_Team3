using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Transform ItemContent;
    public GameObject InventoryItem;
    public static bool Opened = false;
    public GameObject EquipmentInventory;
    public GameObject Inventory;
    public GameObject GeneralInventory;
    public GameObject NumberMenuForDrop;
    public GameObject NumberMenuForUse;
    public InputField InputNumberDrop;
    public InputField InputNumberUse;
    public Transform Player;
    public static Dictionary<Item, int> ItemList = new Dictionary<Item, int>();

    private Text tempName;
    private int DropAmount;


    // Normal Set up
    public void Awake()
    {
        Instance = this;

    }


    /*to get item and drop item*/
    public void Add(Item item)
    {
        if (!ItemList.ContainsKey(item))
        {
            ItemList.Add(item, 1);
        }
        else
        {
            ItemList[item] += 1;
        }
        ListItems();
    }

    public void Remove(Text name, int amount)
    {
        int count = -1;
        foreach(var item in ItemList)
        {
            Item Item = item.Key;
            int value = item.Value;
            if (Item.itemName.Equals(name.text))
            {
                if(value > amount)
                {
                    ItemList[Item] -= amount;
                    DropAmount = amount;
                }
                else
                {
                    ItemList.Remove(Item);
                    DropAmount = value;
                }
                break;
            }
        }
        ListItems();
    }

    public void DropWithNumberMenu(Text name)
    {
        NumberMenuForDrop.SetActive(true);
        tempName = name; 
    }
    public void UseWithNumberMenu(Text name)
    {
        NumberMenuForUse.SetActive(true);
        tempName = name;
    }


    public void DropWithNumber()
    {
        int value = Convert.ToInt32(InputNumberDrop.text);
        Remove(tempName, value);
        DropItem(tempName, DropAmount);
        NumberMenuManager.Instance.Reset();
    }
    public void UseWithNumber()
    {
        int value = Convert.ToInt32(InputNumberUse.text);
        Remove(tempName, value);
        //TODO:have function to use with number
        NumberMenuManager.Instance.Reset();
    }
    /*To create a item dropped*/
    public void DropItem(Text name, int amount)
    {
        for(int i = 0; i<amount; i++)
        {
            GameObject obj = ToolsFactory.Instance.GetDropObject(name);
            Vector3 newPosition = Player.position + (Player.forward * 8) + (Player.up * -4f);
            Instantiate(obj, newPosition, Player.rotation);
        }

    }

    /*update the item in inventory*/
    public void ListItems()
    {
        foreach(Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in ItemList)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            var itemAmount = obj.transform.Find("Amount").GetComponent<Text>();

            Item Item = item.Key;
            int amount = item.Value;
            itemName.text = Item.itemName;
            itemIcon.sprite = Item.icon;
            itemAmount.text = amount + "";
        }
    }






}
