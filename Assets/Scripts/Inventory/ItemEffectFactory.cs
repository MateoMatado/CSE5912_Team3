using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectFactory : MonoBehaviour
{
    public static ItemEffectFactory Instance;
    // Start is called before the first frame update
    public void Awake()
    {
        /*make it Instance*/
        Instance = this;

    }

    public void Effect(string name)
    {
        switch (name)
        {
            case "Red Pot":
                PlayerStatus.Instance.HealthChange(50);
                break;
            case "Blue Pot":
                PlayerStatus.Instance.ManaChange(50);
                break;
            case "Green Pot":
                PlayerStatus.Instance.SpeedChange(30f);
                break;
            case "Bigger Pot":
                PlayerStatus.Instance.GetBigger(30f);
                break;
            case "Potions Red":
                PlayerStatus.Instance.HealthChange(20);
                break;
            case "Potions Blue":
                PlayerStatus.Instance.ManaChange(20);
                break;
            case "Potions Green":
                PlayerStatus.Instance.SpeedChange(10f);
                break;
        }
    }
}
