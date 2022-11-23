using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSave : MonoBehaviour, IData
{
    [SerializeField] private int IslandNumber;
    public void LoadData(GameData data)
    {
        //do nothing
    }

    public void SaveData(ref GameData data)
    {

    }
}
