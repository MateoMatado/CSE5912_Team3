using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemDropAndUse : MonoBehaviour
{

    public Text name;
    public Text Amount;
    public void Drop()
    {
        int value = Convert.ToInt32(Amount);
        if(value == 1)
        {
            Destroy(gameObject);
        }
        InventoryManager.Instance.Remove(name);
        InventoryManager.Instance.DropItem(name);
    }

    public void Use()
    {
        int value = Convert.ToInt32(Amount);
        if (value == 1)
        {
            Destroy(gameObject);
        }
        InventoryManager.Instance.Remove(name);
        //TODO: have a function called for the effect
    }
}
