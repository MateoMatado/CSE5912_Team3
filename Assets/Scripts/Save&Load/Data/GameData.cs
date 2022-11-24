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
    /*Island and its Location*/
    public List<int> Islands;
    public List<Vector3> Location;
    /*Items and its amount*/
    public List<int> Items;
    public List<int> ItemsAmount;
    /*Equipment*/
    public List<int> Equipped;
    public List<int> Equipment;
    public GameData()
    {
        this.HP = 1000;
        this.MP = 100;
        this.Coin = 500;
        playerPosition = new Vector3(33.76378f, 44.6773f, -6.12925f);
        Island = new Dictionary<int, Vector3>();
        Islands = new List<int>();
        Location = new List<Vector3>();
        Items = new List<int>();
        ItemsAmount = new List<int>();
        Equipped = new List<int>();
        Equipment = new List<int>();
    }
}
