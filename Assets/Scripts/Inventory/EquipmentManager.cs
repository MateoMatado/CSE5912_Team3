using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using Team3.Events;

public class EquipmentManager : MonoBehaviour, IData
{
    public static EquipmentManager Instance;
    public List<Item> Equipment = new List<Item>();
    public List<Item> Equipped = new List<Item>();
    public Transform ItemContent;
    public static bool Opened = false;
    public GameObject EquipmentInventory;
    public GameObject Inventoty;
    public GameObject smallInventory;
    public Transform Player;
    public List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> Equipinventory = new List<GameObject>();
    private Item tempEquipment;
    public Sprite NoEquip;
    public List<Image> WeaponHUD;
    public Item initiateWeapon;
    private bool EquipFirst = true;
    private bool EquipSecond = false;
    AvatarIKGoal lastHand = AvatarIKGoal.RightHand;

    /*Save and Load*/
    [SerializeField] private List<int> EquippedSave = new List<int>();
    [SerializeField] private List<int> EquipmentSave = new List<int>();
    // Normal Set up
    public void Awake()
    {
        Equipped.Add(null);
        Equipped.Add(null);
        Add(initiateWeapon);
        Instance = this;

    }
    /*Load Data*/
    public void Start()
    {
        Equipment = new List<Item>();
        Equipped = new List<Item>();
        Equipped.Add(null);
        Equipped.Add(null);
        foreach (int number in EquipmentSave)
        {
            Equipment.Add(ItemsFactory.Instance.GetEquip(number));
        }
        Equipped[0] = ItemsFactory.Instance.GetEquip(EquippedSave[0]);
        Equipped[1] = ItemsFactory.Instance.GetEquip(EquippedSave[1]);

    }

    /*to get item and drop item*/
    public void Add(Item item)
    {
        Equipment.Add(item);
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
                GameObject obj = ItemsFactory.Instance.GetDropObject(name.text);
                Vector3 newPosition = Player.position + (Player.forward * 8) + (Player.up );
                Instantiate(obj, newPosition, Player.rotation);
            }
        }
        ListItems();
    }

    /*Equip item*/
    public void Equip(Text name, int pos)
    {
        for (int i = 0; i < Equipment.Count; i++)
        {
            Item Item = Equipment[i];
            if (Item.itemName.Equals(name.text))
            {
                Equipment.Remove(Item);
                GameObject obj = Equipinventory[i];
                var itemName = obj.transform.Find("Name").GetComponent<Text>();
                if (!itemName.text.Equals("1"))
                {
                    Equipment.Add(Equipped[pos]);
                }
                Equipped[pos] = Item;
                break;
            }
        }

        ListItems();
        EquipWeapon();
    }

    public void TakeOff(int pos)
    {
        GameObject obj = Equipinventory[pos];
        var itemName = obj.transform.Find("Name").GetComponent<Text>();
        if (Equipped[pos] != null)
        {
            Equipment.Add(Equipped[pos]);
            Equipped[pos] = null;
        }
        ListItems();
    }
    /*update the item in inventory*/
    public void ListItems()
    {
        while (Equipment.Contains(null))
        {
            Equipment.Remove(null);
        }
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
        for(int i = 0; i<Equipped.Count; i++)
        {
            Item Item = Equipped[i];
            if(Item != null)
            {
                GameObject obj = Equipinventory[i];
                var itemName = obj.transform.Find("Name").GetComponent<Text>();
                var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
                itemIcon.color = new Color32(255, 255, 255, 255);
                itemName.text = Item.itemName;
                itemIcon.sprite = Item.icon;
                WeaponHUD[i].sprite = Item.icon;
                WeaponHUD[i].color = new Color32(255, 255, 255, 255);
            }
            else if(Item == null)
            {
                GameObject obj = Equipinventory[i];
                var itemName = obj.transform.Find("Name").GetComponent<Text>();
                var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
                itemIcon.color = new Color32(115, 108, 108, 85);
                itemName.text = "1";
                itemIcon.sprite = NoEquip;
            }

        }


    }
    /*Connect System to Player*/
    public void EquipWeapon()
    {
        Item Item = Equipped[0];
        if (EquipFirst)
        {
            Item = Equipped[0];

        }
        if(EquipSecond)
        {
            Item = Equipped[1];
        }
        /*GetWeapon*/
        if (Item != null)
        {
            GetWeapon(Item.itemName);
        }
        else
        {
            GetWeapon("null");
        }
    }

    public void GetWeapon(String name)
    {
        GameObject obj = ItemsFactory.Instance.GetDropObject(name);
        switch (name)
        {
            case "Sword":
                EventsPublisher.Instance.PublishEvent("EquipWeapon", lastHand, new Team3.Animation.Player.Weapons.IKSword(obj));
                break;
            case "FoamFinger":
                EventsPublisher.Instance.PublishEvent("EquipWeapon", lastHand, new Team3.Animation.Player.Weapons.IKFoam(obj));
                break;
            case "Hammer":
                EventsPublisher.Instance.PublishEvent("EquipWeapon", lastHand, new Team3.Animation.Player.Weapons.IKHammer(obj));
                break;
            case "ConfettiGun":
                EventsPublisher.Instance.PublishEvent("EquipWeapon", lastHand, new Team3.Animation.Player.Weapons.IKConfettiGun(obj));
                break;
            case "null":
                EventsPublisher.Instance.PublishEvent("EquipWeapon", lastHand, new Team3.Animation.Player.Weapons.IKUnarmed());
                break;
        }
            

    }

    public void SwitchWeapon()
    {
        if (EquipFirst)
        {
            EquipFirst = false;
            EquipSecond = true;
        }
        else if(!EquipFirst)
        {
            EquipSecond = false;
            EquipFirst = true;
        }
        EquipWeapon();
    }

    public void LoadData(GameData data)
    {
        foreach (int num in data.Equipped)
        {
            EquippedSave.Add(num);
        }
        foreach (int num in data.Equipment)
        {
            EquipmentSave.Add(num);
        }
    }

    public void SaveData(ref GameData data)
    {
        //data.Islands = new List<int>();
        //data.Location = new List<Vector3>();
        data.Equipped = new List<int>();
        data.Equipment = new List<int>();
        foreach (var item in Equipped)
        {
            if(item == null)
            {
                data.Equipped.Add(-1);
            }
            else
            {
                data.Equipped.Add(item.id);
            }

        }
        foreach (var item in Equipment)
        {
            data.Equipment.Add(item.id);
        }

    }
}
