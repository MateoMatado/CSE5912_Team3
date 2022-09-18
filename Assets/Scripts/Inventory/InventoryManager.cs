using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    private GameInput inputs;

    public Transform ItemContent;
    public GameObject InventoryItem;
    public static bool Opened = false;
    public GameObject Inventory;

    public Transform Player;

    public static Dictionary<Item, int> ItemList = new Dictionary<Item, int>();


    // Normal Set up
    public void Awake()
    {
        Instance = this;
        inputs = new GameInput();
        inputs.Player.Inventory.performed += Inventory_performed;
        inputs.Player.Inventory.canceled += Inventory_performed;
    }
    private void OnEnable()
    {
        inputs.Player.Inventory.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Inventory.Disable();
    }

    /*following three functions is to open inventory*/
    private void Inventory_performed(InputAction.CallbackContext context)
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
        Inventory.SetActive(true);
        Opened = true;
    }

    public void Close()
    {
        Inventory.SetActive(false);
        Opened = false;
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

    public void Remove(Text name)
    {
        int count = -1;
        foreach(var item in ItemList)
        {
            Item Item = item.Key;
            int value = item.Value;
            if (Item.itemName.Equals(name.text))
            {
                if(value > 1)
                {
                    ItemList[Item]--;
                }
                else
                {
                    ItemList.Remove(Item);
                }
                break;
            }
        }
        ListItems();

    }
    /*To create a item dropped*/
    public void DropItem(Text name)
    {
        GameObject obj = ToolsFactory.Instance.GetDropObject(name);
        Vector3 newPosition = Player.position + (Player.forward * 8) + (Player.up * -4f);
        Instantiate(obj, newPosition, Player.rotation);
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
