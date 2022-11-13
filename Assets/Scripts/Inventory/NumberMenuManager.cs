using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;


public class NumberMenuManager : MonoBehaviour
{
    public static NumberMenuManager Instance;
    public InputField InputNumberForUse;
    public InputField InputNumberForDrop;
    public InputField InputNumberForShop;
    private static int count = 1;

    // Normal Set up
    public void Awake()
    {
        Instance = this;

    }
    public void Reset()
    {
        count = 1;
    }

    public void Increase()
    {
        count++;    
    }

    public void Decrease()
    {
        if(count > 0)
        {
            count--;
        }
        
    }

    public void Update()
    {
        InputNumberForUse.text = count + "";
        InputNumberForDrop.text = count + "";
        InputNumberForShop.text = count + "";
    }
}
