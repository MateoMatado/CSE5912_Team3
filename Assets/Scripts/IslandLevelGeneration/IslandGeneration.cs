using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField] private int Bias = 3;
    [SerializeField] private int Width = 100;
    [SerializeField] private int Length = 100;
    [SerializeField] private int Height = 50;
    [SerializeField] private int Attempts = 10;
    [SerializeField] private float Spacing = 2;
    [SerializeField] private float RandomHeightLimit = 1000;
    [SerializeField] private List<GameObject> Islands;

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
        int BiasDec = Bias;
        while (BiasDec >= 0)
        {
            for (int i = 0; i < Attempts; i++)
            {
                int IslRand = Random.Range(0, Islands.Count);
                for (int j = 0; j < Bias; j++)
                {
                    int IslRand2 = Random.Range(0, Islands.Count - 2);
                    if (IslRand2 > IslRand)
                    {
                        IslRand = IslRand2;
                    }
                }

                Vector2 IslandSize = DIslands[Islands[IslRand]];
                Vector2 pos = new Vector2(Random.Range(0, Width), Random.Range(0, Length));
                float rot = Random.Range(0, 4);

                bool check = IslandCheck(pos, IslandSize);
                bool rotcheck = false;

                if (check)
                {
                    gameObjects[(int)pos.x, (int)pos.y] = GameObject.Instantiate(Islands[IslRand]);
                    if (rot == 0)
                    {
                        gameObjects[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                        rotcheck = true;
                    }
                    else if (rot == 1)
                    {
                        gameObjects[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, 180, 0), Space.Self);
                    }
                    else if (rot == 2)
                    {
                        gameObjects[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                        rotcheck = true;
                    }
                    gameObjects[(int)pos.x, (int)pos.y].transform.position = new Vector3((int)pos.x * Spacing, Random.Range(100, RandomHeightLimit), (int)pos.y * Spacing);
                    gameObjects[(int)pos.x, (int)pos.y].transform.SetParent(transform);

                    if (rotcheck)
                    {
                        for (int x = 0; x < IslandSize.x; x++)
                        {
                            for (int y = 0; y < IslandSize.y; y++)
                            {
                                EIslandArray[x + (int)pos.x, y + (int)pos.y] = (int)EIslands.Filled;
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < IslandSize.y; x++)
                        {
                            for (int y = 0; y < IslandSize.x; y++)
                            {
                                EIslandArray[x + (int)pos.x, y + (int)pos.y] = (int)EIslands.Filled;
                            }
                        }
                    }

                }
            }
            BiasDec--;
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
        for (int x = 0; x < IslandSize.y; x++)
        {
            for (int y = 0; y < IslandSize.x; y++)
            {
                if (x + (int)pos.x >= Width || y + (int)pos.y >= Length || EIslandArray[x + (int)pos.x, y + (int)pos.y] == (int)EIslands.Filled)
                {
                    check = false;
                }
            }
        }
        return check;
    }
}
