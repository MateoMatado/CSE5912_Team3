using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class InventoryManager : MonoBehaviour
{
    /*inventory item*/
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
    /*Quick items*/
    public GameObject QuickItemsSystem;
    public List<String> QuickItems = new List<String>();
    [SerializeField] public List<GameObject> Keys = new List<GameObject>();
    [SerializeField] public List<GameObject> HUDKeys = new List<GameObject>();
    /*temp variable*/
    private Text tempName;
    private int DropAmount;
    private Text tempAmount;
    private Image tempIcon;

    /*information box*/
    private GameObject currentInformation = null;
    public GameObject informationBox;
    public Transform canvas;
    // Normal Set up
    public void Awake()
    {
        QuickItems.Add(null);
        QuickItems.Add(null);
        QuickItems.Add(null);
        QuickItems.Add(null);
        /*make it Instance*/
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

    /*open Menu*/
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
    public void EquipItem(Text name, Image Icon, Text Amount)
    {
        QuickItemsSystem.SetActive(true);
        tempName = name;
        tempAmount = Amount;
        tempIcon = Icon;
    }

    /*Equip Item to Quick Key*/
    public void Equip(int position)
    {
        GameObject obj = Keys[position];
        var itemName = obj.transform.Find("Name").GetComponent<Text>();
        var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
        var itemAmount = obj.transform.Find("Amount").GetComponent<Text>();
        itemName.text = tempName.text;
        itemIcon.color = new Color32(255, 255, 255, 255);
        itemIcon.sprite = tempIcon.sprite;
        itemAmount.text = tempAmount.text;
        QuickItems[position] = itemName.text;
        
        ChangeQuickItem(position);
    }
    public void Equip1()
    {
        Equip(0);
    }
    public void Equip2()
    {
        Equip(1);
    }
    public void Equip3()
    {
        Equip(2);
    }
    public void Equip4()
    {
        Equip(3);
    }
    /*Operation of Number Menu*/
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
            Instantiate(obj, newPosition, obj.transform.rotation);
        }

    }
    /*Display information box*/
    public void Display(string name, string effect, Vector3 position)
    {
        if(currentInformation != null)
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
    /*update the item in inventory*/
    public void ListItems()
    {
        /*sort the Inventory*/
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
            var itemEffect = obj.transform.Find("Effect").GetComponent<Text>();

            Item Item = item.Key;
            int amount = item.Value;
            itemName.text = Item.itemName;
            itemIcon.sprite = Item.icon;
            itemAmount.text = amount + "";
            itemEffect.text = Item.Effect;
        }
    }
    public void ChangeQuickItem(int position)
    {

        GameObject obj = Keys[position];
        var itemName = obj.transform.Find("Name").GetComponent<Text>();
        var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
        var itemAmount = obj.transform.Find("Amount").GetComponent<Text>();

        GameObject obj2 = HUDKeys[position];
        var Name = obj2.transform.Find("Name").GetComponent<Text>();
        var Icon = obj2.transform.Find("Icon").GetComponent<Image>();
        var Amount = obj2.transform.Find("Amount").GetComponent<Text>();
        Icon.color = new Color32(255, 255, 255, 255);
        Name.text = itemName.text;
        Icon.sprite = itemIcon.sprite;
        Amount.text = itemAmount.text;
        
        
    }






}
