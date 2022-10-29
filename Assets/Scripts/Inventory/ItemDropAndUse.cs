using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemDropAndUse : MonoBehaviour, IPointerClickHandler
{

    public Text name;
    public Text Amount;
    public Image Icon;
    public Text Effect;
    public GameObject UseButton;
    public GameObject DropButton;
    public GameObject EquipButton;

    public void DropMenu()
    {
        InventoryManager.Instance.DropWithNumberMenu(name);
    }
    public void UseMenu()
    {
        InventoryManager.Instance.UseWithNumberMenu(name);
    }
    public void EquipQuickItem()
    {
        InventoryManager.Instance.EquipItem(name, Icon, Amount);
    }
    public void DisplayInformation()
    {
        InventoryManager.Instance.Display(name.text, Effect.text, transform.position);
    }
    public void NotDisplayInformation()
    {
        InventoryManager.Instance.NotDisplay();
    }
    public void Use()
    {
        int value = Convert.ToInt32(Amount);
        if (value == 1)
        {
            Destroy(gameObject);
        }
        InventoryManager.Instance.Remove(name,1);
        ItemEffectFactory.Instance.Effect(name.text);
    }

    public void QuickUse()
    {
       
        InventoryManager.Instance.Remove(name, 1);
        ItemEffectFactory.Instance.Effect(name.text);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseButton.SetActive(true);
            DropButton.SetActive(true);
            EquipButton.SetActive(true);
        }
    }
}
