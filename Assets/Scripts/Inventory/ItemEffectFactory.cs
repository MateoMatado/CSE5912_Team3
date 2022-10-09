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
                PlayerStatus.Instance.HealthChange(20);
                break;
            case "Blue Pot":
                PlayerStatus.Instance.ManaChange(20);
                break;
        }
    }
}
