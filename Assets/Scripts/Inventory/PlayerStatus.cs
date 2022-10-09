using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    public static float HP = 80;
    public static float Mana = 80;
    private static float ChangeValueHP = 0;
    private static float ChangeValueMana = 0;
    // Normal Set up
    public void Awake()
    {
        /*make it Instance*/
        Instance = this;

    }
    public float GetValue(string name)
    {
        switch (name){
            case "HP":
                return HP;
                break;
            case "Mana":
                return Mana;
                break;
            default:
                return -1;
        }
        return -1;
    }

    public void HealthChange(float amount)
    {
        ChangeValueHP = amount;
    }

    public void ManaChange(float amount)
    {
        ChangeValueMana = amount;
    }

    public void Update()
    {
        if(ChangeValueHP != 0)
        {
            if(ChangeValueHP > 0)
            {
                HP += 0.2f;
                ChangeValueHP -= 0.2f;
                
            }
            else
            {
                HP -= 0.2f;
                ChangeValueHP  += 0.2f;
            }
        }
        if (ChangeValueMana != 0)
        {
            if (ChangeValueMana > 0)
            {
                Mana += 0.2f;
                ChangeValueMana -= 0.2f;

            }
            else
            {
                Mana -= 0.2f;
                ChangeValueMana += 0.2f;
            }
        }
    }
}
