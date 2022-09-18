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


    // Start is called before the first frame update
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
        Time.timeScale = 0f;
        Opened = true;
    }

    public void Close()
    {
        Inventory.SetActive(false);
        Time.timeScale = 1f;
        Opened = false;
    }
    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Text name)
    {
        int count = -1;
        for(int i = 0; i< Items.Count; i++)
        {
            Item item = Items[i];
            if(item.itemName.Equals(name.text))
            {
                Items.Remove(item);
                break;
            }
        }
        GameObject obj = ToolsFactory.Instance.GetDropObject(name);
        Vector3 newPosition = Player.position + (Player.forward * 8) + (Player.up *-4f);
        Instantiate(obj, newPosition, Player.rotation);

    }

    public void ListItems()
    {
        foreach(Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }




}
