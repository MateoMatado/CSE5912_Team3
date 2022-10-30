using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance = new PlayerStatus();
    public static float HP = 20;
    public static float Mana = 20;
    private static float ChangeValueHP = 0;
    private static float ChangeValueMana = 0;
    private static float AccelerateTime = 0;
    private static float BiggerTime = 0;
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
    /*Change HP or Mana*/
    public void HealthChange(float amount)
    {
        ChangeValueHP += amount;
    }

    public void ManaChange(float amount)
    {
        ChangeValueMana += amount;
    }

    /*Accelerate*/
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

    /*Get Bigger*/
    public void GetBigger(float amount)
    {
        transform.localScale = new Vector3(4, 4, 4);
        BiggerTime = amount;
        transform.position += transform.up * 5f;
        StartCoroutine(Bigger());
    }
    IEnumerator Bigger()
    {

        while (BiggerTime != 0)
        {
            BiggerTime--;
            yield return new WaitForSeconds(1);
        }
        transform.localScale = new Vector3(3, 3, 3);
        transform.position += transform.up * 25f;
    }

    /*Normal Update*/
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
