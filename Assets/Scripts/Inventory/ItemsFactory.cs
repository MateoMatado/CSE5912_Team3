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
    /*Items for inventory*/
    [SerializeField] Item RedPot1;
    [SerializeField] Item RedPotions1;
    [SerializeField] Item BluePot1;
    [SerializeField] Item BluePotions1;
    [SerializeField] Item GreenPot1;
    [SerializeField] Item GreenPotions1;
    [SerializeField] Item BerserkPotions1;
    [SerializeField] Item BiggerPot1;
    [SerializeField] Item PurplePot1;
    /*Items for Equipment*/
    [SerializeField] Item Sword1;
    [SerializeField] Item FoamFinger1;
    [SerializeField] Item Hammer1;
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

    public Item GetItem(int id)
    {
        Item res = null;
        switch (id)
        {
            case 0:
                res = PurplePot1;
                break;
            case 1:
                res = RedPot1;
                break;
            case 2:
                res = BluePot1;
                break;
            case 3:
                res = GreenPot1;
                break;
            case 4:
                res = RedPotions1;
                break;
            case 5:
                res = BluePotions1;
                break;
            case 6:
                res = GreenPotions1;
                break;
            case 7:
                res = BerserkPotions1;
                break;
            case 8:
                res = BiggerPot1;
                break;
        }
        return res;
    }

    public Item GetEquip(int id)
    {
        Item res = null;
        switch (id)
        {
            case -1:
                res = null;
                break;
            case 102:
                res = Sword1;
                break;
            case 106:
                res = FoamFinger1;
                break;
            case 105:
                res = Hammer1;
                break;
        }
        return res;
    }
}
