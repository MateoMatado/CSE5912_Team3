using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField] private int Width = 100;
    [SerializeField] private int Length = 100;
    [SerializeField] private int Height = 50;
    [SerializeField] private int Attempts = 10;
    [SerializeField] private int Spacing = 2;
    [SerializeField] private float RandomHeightLimit = 1000;
    [SerializeField] private List<GameObject> Islands;
    [SerializeField] private GameObject StartingIsland;

    Vector2 SislandSize = new Vector2(1000,1000);

    private Dictionary<GameObject,Vector2> DIslands = new Dictionary<GameObject,Vector2>();
    private enum EIslands{Empty,Filled};
    private int[,] EIslandArray;
    private GameObject[,] gameObjects;



    // Start is called before the first frame update
    void Start()
    {
        DIslands.Add(Islands[0], new Vector2(250, 200));
        DIslands.Add(Islands[1], new Vector2(500, 500));
        DIslands.Add(Islands[2], new Vector2(800, 600));
        DIslands.Add(Islands[3], new Vector2(800, 800));

        Generate();

    }

    private void Generate()
    {
        EIslandArray = new int[Width, Length];
        gameObjects = new GameObject[Width, Length];
        
        for (int i = 0; i < Attempts; i++)
        {
            int IslRand = Random.Range(0, Islands.Count);
            Vector2 IslandSize = DIslands[Islands[IslRand]];
            Vector2 pos = new Vector2(Random.Range(0, Width), Random.Range(0, Length));

            bool check = IslandCheck(pos, IslandSize);

            for (int x = 0; x < SislandSize.x; x++)
            {
                for (int y = 0; y < SislandSize.y; y++)
                {
                    EIslandArray[x + (int)pos.x, y + (int)pos.y] = (int)EIslands.Filled;
                }
            }
            gameObjects[100, 100] = GameObject.Instantiate(StartingIsland);
            gameObjects[100, 100].transform.position = new Vector3(100 * Spacing, Random.Range(100,RandomHeightLimit),100 * Spacing);
            gameObjects[100, 100].transform.SetParent(transform);

            if (check)
            {
                for (int x = 0; x < IslandSize.x; x++)
                {
                    for (int y = 0; y < IslandSize.y; y++)
                    {
                        EIslandArray[x + (int)pos.x, y + (int)pos.y] = (int)EIslands.Filled;
                    }
                }
                gameObjects[(int)pos.x, (int)pos.y] = GameObject.Instantiate(Islands[IslRand]);
                gameObjects[(int)pos.x, (int)pos.y].transform.position = new Vector3((int)pos.x * Spacing, Random.Range(100,RandomHeightLimit),(int)pos.y * Spacing);
                gameObjects[(int)pos.x, (int)pos.y].transform.SetParent(transform);
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
