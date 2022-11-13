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
    public GameObject CD;
    public Slider cd;
    public bool inCD = false;

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
    /*shop part*/
    public void ShopDropMenu()
    {
        ShopManager.Instance.DropWithNumberMenu(name);
    }
    public void ShopDisplayInformation()
    {
        ShopManager.Instance.Display(name.text, Effect.text, transform.position);
    }
    public void ShopNotDisplayInformation()
    {
        ShopManager.Instance.NotDisplay();
    }
    public void Use()
    {
        if (!ItemEffectFactory.Instance.InCD())
        {
            int value = Convert.ToInt32(Amount);
            if (value == 1)
            {
                Destroy(gameObject);
            }
            InventoryManager.Instance.Remove(name, 1);
            ItemEffectFactory.Instance.Effect(name.text);
        }

    }

    public void QuickUse()
    {
        if (!ItemEffectFactory.Instance.InCD())
        {
            InventoryManager.Instance.Remove(name, 1);
            ItemEffectFactory.Instance.Effect(name.text);
        }


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

    public void Update()
    {
        if (ItemEffectFactory.Instance.InCD() && !inCD)
        {
            inCD = true;
            CD.SetActive(true);
            cd.value = 1f;
            StartCoroutine(InCD());
        }
    }
    IEnumerator InCD()
    {
        while(cd.value != 0f)
        {
            cd.value -= 0.02f;
            yield return new WaitForSeconds(0.05f);
        }
        CD.SetActive(false);
        inCD = false;
    }
}
