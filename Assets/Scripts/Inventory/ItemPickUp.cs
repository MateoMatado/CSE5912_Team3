using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    // Start is called before the first frame update
    void PickUp()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        PickUp();
    }

}
