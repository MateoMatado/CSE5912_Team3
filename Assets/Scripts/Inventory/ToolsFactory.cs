using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsFactory : MonoBehaviour
{
    public static ToolsFactory Instance;
    [SerializeField] GameObject RedPot;
    [SerializeField] GameObject BluePot;
    [SerializeField] GameObject GreenPot;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public GameObject GetDropObject(Text Name)
    {
        GameObject obj;
        switch (Name.text)
        {
            case "Red Pot":
                obj = RedPot;
                break;
            case "Blue Pot":
                obj = BluePot;
                break;
            case "Green Pot":
                obj = GreenPot;
                break;

            default:
                obj = null;
                break;

        }
        return obj;
            
    }
}
