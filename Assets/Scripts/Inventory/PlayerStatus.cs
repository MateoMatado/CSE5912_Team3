using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance = new PlayerStatus();
    public static float HP = 80;
    public static float Mana = 80;
    private static float ChangeValueHP = 0;
    private static float ChangeValueMana = 0;
    private static float AccelerateTime = 0;
    private static float Speed = 1f;
    private static float time =0;
    [SerializeField] private float ChangeSpeed = 0.2f;
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
            case "Speed":
                return Speed;
                break;
            default:
                return -1;
        }
        return -1;
    }

    public void HealthChange(float amount)
    {
        ChangeValueHP += amount;
    }

    public void ManaChange(float amount)
    {
        ChangeValueMana += amount;
    }
    public void SpeedChange(float amount)
    {
        Speed = 2f;
        AccelerateTime = amount;
        StartCoroutine(Accelerate());
    }
    IEnumerator Accelerate()
    {
        
        while (AccelerateTime != 0)
        {
            AccelerateTime--;
            yield return new WaitForSeconds(1);
        }
        Speed = 1f;
    }
    public void Update()
    {
        if(ChangeValueHP != 0)
        {
            if(ChangeValueHP > 0)
            {
                HP +=ChangeSpeed;
                ChangeValueHP -=ChangeSpeed;
                
            }
            else
            {
                HP -=ChangeSpeed;
                ChangeValueHP  +=ChangeSpeed;
            }
        }
        if (ChangeValueMana != 0)
        {
            if (ChangeValueMana > 0)
            {
                Mana +=ChangeSpeed;
                ChangeValueMana -=ChangeSpeed;

            }
            else
            {
                Mana -=ChangeSpeed;
                ChangeValueMana +=ChangeSpeed;
            }
        }
    }
}
