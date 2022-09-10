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

    private Dictionary<GameObject,Vector2> DIslands = new Dictionary<GameObject,Vector2>();
    private enum EIslands{Empty,Filled};
    private int[,] EIslandArray;
    private GameObject[,] gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        DIslands.Add(Islands[0], new Vector2(2, 2));
        DIslands.Add(Islands[1], new Vector2(4, 4));

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

        for (int i = 0; i < Attempts; i++)
        {
            Vector2 IslandSize = DIslands[Islands[Random.Range(0, Islands.Count)]];
            Vector2 pos = new Vector2(Random.Range(0,Width), Random.Range(0,Length));

            bool check = IslandCheck(pos,IslandSize);

            if (check)
            {
                for (int x = 0; x < IslandSize.x; x++)
                {
                    for (int y = 0; y < IslandSize.y; y++)
                    {
                        GameObject.Destroy(gameObjects[x+ (int)pos.x, y+ (int)pos.y]);
                        gameObjects[x, y] = GameObject.Instantiate(Islands[0]);
                        gameObjects[x, y].transform.position = new Vector3(x + (int)pos.x, 0, y + (int)pos.y);
                        gameObjects[x, y].transform.SetParent(transform);
                    }
                }
            }
        }

    }

    private Boolean IslandCheck(Vector2 pos, Vector2 IslandSize)
    {
        bool check = true;
        for (int x = 0; x < IslandSize.x; x++)
        {
            for (int y = 0; y < IslandSize.y; y++)
            {
                if (x + (int)pos.x >= Width || y + (int)pos.y >= Length || EIslandArray[x+ (int)pos.x, y+ (int)pos.y] == (int)EIslands.Filled)
                {
                    check = false;
                }
            }
        }
        return check;
    }
}
