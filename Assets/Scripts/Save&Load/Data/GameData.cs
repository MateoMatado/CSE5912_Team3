using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class GameData
{

    public float HP;
    public float MP;
    public int Coin;
    public Vector3 playerPosition;
    public Dictionary<int, Vector3> Island;
    public List<int> Islands;
    public List<Vector3> Location;
    public GameData()
    {
        this.HP = 1000;
        this.MP = 100;
        this.Coin = 500;
        
        Island = new Dictionary<int, Vector3>();
        Islands = new List<int>();
        Location = new List<Vector3>();
    }
}
