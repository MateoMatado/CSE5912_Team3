using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField] private int Width = 100;
    [SerializeField] private int Length = 100;
    [SerializeField] private int  Height = 50;

    [SerializeField] private int Attempts = 10;

    [SerializeField] private List<GameObject> Islands;
    private enum EIslands{Empty,Filled};
    private int[,] EIslandArray;
    private GameObject[,] gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        EIslandArray = new int[Width,Length];
        gameObjects = new GameObject[Width, Length];
        for(int i = 0; i < Width; i++)
        {
            for(int j = 0; j < Length; j++)
            {
                EIslandArray[i,j] = (int)EIslands.Empty;
                gameObjects[i, j] = GameObject.Instantiate(Islands[1]);
                gameObjects[i, j].transform.position = new Vector3(i,0,j);
                gameObjects[i, j].transform.SetParent(transform);
            }
        }
        for(int i = 0; i <= Attempts; i++)
        {
            Console.WriteLine(EIslandArray[Random.Range(0,Width),Random.Range(0,Length)]);
        }
        
    }
}
