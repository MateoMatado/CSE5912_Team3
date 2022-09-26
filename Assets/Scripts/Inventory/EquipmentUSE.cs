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
        EquipmentManager.Instance.Equip1(name);

    }
    public void Equip2()
    {
        EquipmentManager.Instance.Equip2(name);

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
