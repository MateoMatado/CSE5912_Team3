using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public float HP;
    public float MP;
    public int Coin;
    public Vector3 playerPosition;

    public GameData()
    {
        this.HP = 50;
        this.MP = 50;
        this.Coin = 500;
    }
}
