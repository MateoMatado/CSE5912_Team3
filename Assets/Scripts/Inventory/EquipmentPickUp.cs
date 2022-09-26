using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickUp : MonoBehaviour
{
    public Item Item;
    // Start is called before the first frame update
    void PickUp()
    {
        EquipmentManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        PickUp();
    }
}
