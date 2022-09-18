using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{

    public Text name;
    public Transform Player;
    public void Drop()
    {
        InventoryManager.Instance.Remove(name);
        Destroy(gameObject);
        
        

    }
}
