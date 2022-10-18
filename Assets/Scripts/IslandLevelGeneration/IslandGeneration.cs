using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField] private int Bias = 3;
    [SerializeField] private int Width = 100;
    [SerializeField] private int Length = 100;
    [SerializeField] private int Height = 50;
    [SerializeField] private int Attempts = 10;
    [SerializeField] private int ChainNum = 15;
    [SerializeField] private float RandomHeightLimit = 1000;
    [SerializeField] private List<GameObject> Islands;
    [SerializeField] private GameObject Chainlink;
    [SerializeField] private GameObject StartingIsland;

    private Dictionary<String,Vector2> DIslands = new Dictionary<String,Vector2>();
    private enum EIslands{Empty,Filled};
    private int[,] EIslandArray;
    private GameObject[,] gameObjectIslands;

    [SerializeField] private List<GameObject> IslandList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DIslands.Add(Islands[0].name.ToString(), new Vector2(250, 200));
        DIslands.Add(Islands[1].name.ToString(), new Vector2(500, 500));
        DIslands.Add(Islands[2].name.ToString(), new Vector2(800, 600));
        DIslands.Add(Islands[3].name.ToString(), new Vector2(800, 800));
        DIslands.Add(Islands[4].name.ToString(), new Vector2(250, 200));

        GenerateIslands();
        GenerateChains();
        
    }
    private void GenerateChains()
    {
        Vector3 Pos1 = new Vector3();
        Vector3 Pos2 = new Vector3();
        int num = 0;
        foreach(GameObject Island in IslandList)
        {
            foreach (GameObject Island2 in IslandList)
            {
                if(Random.RandomRange(0,100) <= 3 && num < ChainNum)
                {
                    GameObject Parent = new GameObject();
                    Parent.name = "Chain";
                    Vector3 pos1 = new Vector3(Island.transform.position.x, Island.transform.position.y, Island.transform.position.z);
                    Vector3 pos2 = new Vector3(Island2.transform.position.x, Island2.transform.position.y, Island2.transform.position.z);
                    ChainScript Chain = new ChainScript(pos1, pos2, Parent, Chainlink);
                    num++;
                }
            }
        }
    }

    private void GenerateIslands()
    {
        EIslandArray = new int[Width, Length];
        gameObjectIslands = new GameObject[Width, Length];
        int BiasDec = Bias;

        for (int x = 0; x < DIslands["StartingIsland"].x; x++)
        {
            for (int y = 0; y < DIslands["StartingIsland"].y; y++)
            {
                EIslandArray[x + 2000, y + 2000] = (int)EIslands.Filled;
            }
        }
        gameObjectIslands[2000, 2000] = GameObject.Instantiate(StartingIsland);
        gameObjectIslands[2000, 2000].transform.position = new Vector3(2000, Random.Range(100, RandomHeightLimit), 2000);
        gameObjectIslands[2000, 2000].transform.SetParent(transform);

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

                Vector2 IslandSize = DIslands[Islands[IslRand].name.ToString()];
                Vector2 pos = new Vector2(Random.Range(0, Width), Random.Range(0, Length));
                float rot = Random.Range(0, 4);

                bool check = IslandCheck(pos, IslandSize);
                bool rotcheck = false;

                if (check)
                {
                    gameObjectIslands[(int)pos.x, (int)pos.y] = GameObject.Instantiate(Islands[IslRand]);
                    gameObjectIslands[(int)pos.x, (int)pos.y].name = Islands[IslRand].name;
                    if (rot == 0)
                    {
                        gameObjectIslands[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                        rotcheck = true;
                    }
                    else if (rot == 1)
                    {
                        gameObjectIslands[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, 180, 0), Space.Self);
                    }
                    else if (rot == 2)
                    {
                        gameObjectIslands[(int)pos.x, (int)pos.y].transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                        rotcheck = true;
                    }
                    gameObjectIslands[(int)pos.x, (int)pos.y].transform.position = new Vector3((int)pos.x, Random.Range(100, RandomHeightLimit), (int)pos.y);
                    gameObjectIslands[(int)pos.x, (int)pos.y].transform.SetParent(transform);

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
                    IslandList.Add(gameObjectIslands[(int)pos.x, (int)pos.y]);
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
