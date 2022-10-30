using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsFactory : MonoBehaviour
{
    public static ItemsFactory Instance;
    /*pot and potions*/
    [SerializeField] GameObject RedPot;
    [SerializeField] GameObject RedPotions;
    [SerializeField] GameObject BluePot;
    [SerializeField] GameObject BluePotions;
    [SerializeField] GameObject GreenPot;
    [SerializeField] GameObject GreenPotions;
    [SerializeField] GameObject BerserkPotions;
    [SerializeField] GameObject BiggerPot;
    [SerializeField] GameObject PurplePot;
    /*Equipment*/
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject FoamFinger;
    [SerializeField] GameObject Hammer;
    /*coin*/
    [SerializeField] GameObject coin;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public GameObject GetDropObject(string Name)
    {
        GameObject obj;
        switch (Name)
        {
            /*Pot*/
            case "Red Pot":
                obj = RedPot;
                break;
            case "Blue Pot":
                obj = BluePot;
                break;
            case "Green Pot":
                obj = GreenPot;
                break;
            case "Bigger Pot":
                obj = BiggerPot;
                break;
            case "Purple Pot":
                obj = PurplePot;
                break;
            /*Potions*/
            case "Potions Red":
                obj = RedPotions;
                break;
            case "Potions Blue":
                obj = BluePotions;
                break;
            case "Potions Green":
                obj = GreenPotions;
                break;
            case "Potions Berserk":
                obj = BerserkPotions;
                break;
            /*Equipment*/
            case "Sword":
                obj = Sword;
                break;
            case "FoamFinger":
                obj = FoamFinger;
                break;
            case "Hammer":
                obj = Hammer;
                break;
            default:
                obj = null;
                break;

        }
        return obj;
    }

    public GameObject GetRandomObject(int NO)
    {
        GameObject obj;
        switch (NO)
        {
            /*Pot*/
            case 1:
                obj = RedPot;
                break;
            case 2:
                obj = BluePot;
                break;
            case 3:
                obj = GreenPot;
                break;
            case 4:
                obj = BiggerPot;
                break;
            case 5:
                obj = PurplePot;
                break;
            /*Potions*/
            case 6:
                obj = RedPotions;
                break;
            case 7:
                obj = BluePotions;
                break;
            case 8:
                obj = GreenPotions;
                break;
            case 9:
                obj = BerserkPotions;
                break;
            /*Equipment*/
            case 10:
                obj = Sword;
                break;
            case 11:
                obj = FoamFinger;
                break;
            case 12:
                obj = Hammer;
                break;
            default:
                obj = coin;
                break;

        }
        return obj;
    }
}
