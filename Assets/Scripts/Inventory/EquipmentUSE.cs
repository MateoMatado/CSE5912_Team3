using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class EquipmentUSE : MonoBehaviour, IPointerClickHandler
{

    public Text name;
    public GameObject UseButton;
    public GameObject DropButton;

    public void Equip1()
    {
        EquipmentManager.Instance.Equip(name,0);

    }
    public void Equip2()
    {
        EquipmentManager.Instance.Equip(name,1);
    }
    public void OFF1()
    {
        EquipmentManager.Instance.TakeOff(0);
    }
    public void OFF2()
    {
        EquipmentManager.Instance.TakeOff(1);
    }

    public void Drop()
    {

        EquipmentManager.Instance.Drop(name);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Drop();
        }
    }


}
