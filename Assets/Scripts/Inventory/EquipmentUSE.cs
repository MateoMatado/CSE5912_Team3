using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class EquipmentUSE : MonoBehaviour
{

    public Text name;
    public GameObject UseButton;
    public GameObject DropButton;

    public void Equip()
    {
        EquipmentManager.Instance.Equip(name);

    }

    public void Drop()
    {

        EquipmentManager.Instance.Drop(name);


    }


}
