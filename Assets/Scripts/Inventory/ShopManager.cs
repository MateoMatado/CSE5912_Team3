using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public Transform Desk;
    public Transform ItemContent;
    public GameObject InventoryItem;
    public static Dictionary<Item, int> ItemList = new Dictionary<Item, int>();
    public GameObject Shop;
    public InputField InputNumberDrop;
    public GameObject NumberMenuForDrop;
    /*temp variable*/
    private Text tempName;
    private int DropAmount;

    /*information box*/
    private GameObject currentInformation = null;
    public GameObject informationBox;
    public Transform canvas;

    public void Awake()
    {
        /*make it Instance*/
        Instance = this;

    }
    public void Active()
    {
        Shop.SetActive(true);
    }
    /*to get item and drop item*/
    public void Add(Item item)
    {
        if (!ItemList.ContainsKey(item))
        {
            ItemList.Add(item, 999);
        }
        else
        {
            ItemList[item] += 1;
        }
        ListItems();
    }

    public void Remove(Text name, int amount)
    {

        foreach (var item in ItemList)
        {
            Item Item = item.Key;
            int value = item.Value;
            if (Item.itemName.Equals(name.text))
            {
                if (value > amount)
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
    /*Display information box*/
    public void Display(string name, string effect, Vector3 position)
    {
        if (currentInformation != null)
        {
            Destroy(currentInformation.gameObject);
        }
        position.x += 165;
        position.y -= 10;
        currentInformation = Instantiate(informationBox, position, Quaternion.identity, canvas);
        var itemName = currentInformation.transform.Find("Name").GetComponent<Text>();
        var itemEffect = currentInformation.transform.Find("Effect").GetComponent<Text>();

        itemName.text = name + ":";
        itemEffect.text = effect;
    }
    public void NotDisplay()
    {
        if (currentInformation != null)
        {
            Destroy(currentInformation.gameObject);
        }
    }
    /*open Menu*/
    public void DropWithNumberMenu(Text name)
    {
        NumberMenuForDrop.SetActive(true);
        tempName = name;
    }
    /*Operation of Number Menu*/
    public void DropWithNumber()
    {
        int value = Convert.ToInt32(InputNumberDrop.text);
        Remove(tempName, value);
        DropItem(tempName, DropAmount);
        NumberMenuManager.Instance.Reset();
    }
    /*To create a item dropped*/
    public void DropItem(Text name, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = ItemsFactory.Instance.GetDropObject(name.text);
            Vector3 newPosition = Desk.position + (Desk.forward * 2f) + (Desk.up * 2f) + (Desk.right * UnityEngine.Random.Range(0f, 5f));
            Instantiate(obj, newPosition, obj.transform.rotation);
        }
    }
    /*update the item in inventory*/
    public void ListItems()
    {
        /*sort the Inventory*/
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in ItemList)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            var itemAmount = obj.transform.Find("Amount").GetComponent<Text>();
            var itemEffect = obj.transform.Find("Effect").GetComponent<Text>();

            Item Item = item.Key;
            int amount = item.Value;
            itemName.text = Item.itemName;
            itemIcon.sprite = Item.icon;
            itemAmount.text = amount + "";
            itemEffect.text = Item.Effect;
        }
    }
}
