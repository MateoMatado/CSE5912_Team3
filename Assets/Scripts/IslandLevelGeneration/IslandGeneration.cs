using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class IslandGeneration : MonoBehaviour, IData
{
    public static IslandGeneration Instance;
    [SerializeField] private int Bias = 3;
    [SerializeField] private int Width = 100;
    [SerializeField] private int Length = 100;
    [SerializeField] private int Height = 50;
    [SerializeField] private int Attempts = 10;
    [SerializeField] private int ChainNum = 15;
    [SerializeField] private float RandomHeightLimit = 1000;
    [SerializeField] private List<GameObject> Islands;
    [SerializeField] private GameObject Chainlink;
    [SerializeField] private GameObject FloatingRock;
    [SerializeField] private GameObject StartingIsland;
    [SerializeField] private GameObject BossIsland;
    [SerializeField] private GameObject ShopIsland;

    private Dictionary<String,Vector2> DIslands = new Dictionary<String,Vector2>();
    private enum EIslands{Empty,Filled};
    private int[,] EIslandArray;
    private GameObject[,] gameObjectIslands;

    [SerializeField] private List<GameObject> IslandList = new List<GameObject>();
    //save and load
    //private Dictionary<int, Vector3> Island = new Dictionary<int, Vector3>();
    [SerializeField] private List<int> Island = new List<int>();
    [SerializeField] private List<Vector3> Location = new List<Vector3>();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        DIslands.Add(Islands[0].name.ToString(), new Vector2(250, 200));
        DIslands.Add(Islands[1].name.ToString(), new Vector2(500, 500));
        DIslands.Add(Islands[2].name.ToString(), new Vector2(500, 500));
        DIslands.Add(Islands[3].name.ToString(), new Vector2(800, 600));
        DIslands.Add(Islands[4].name.ToString(), new Vector2(800, 800));
        DIslands.Add(Islands[5].name.ToString(), new Vector2(800, 800));
        DIslands.Add(Islands[6].name.ToString(), new Vector2(800, 600));
        DIslands.Add(Islands[7].name.ToString(), new Vector2(350, 350));
        if (Island.Count != 0)
        {
            Debug.Log("Total Island: " + Island.Count);
            for (int i = 0; i< Island.Count; i++)
            {
                LoadIsland(Island[i],Location[i]);
            }
        }
        else
        {
            GenerateIslands();
            GenerateChains();
        }

        
    }
    private void GenerateChains()
    {
        Vector3 Pos1 = new Vector3();
        Vector3 Pos2 = new Vector3();
        int num = 0;
        int posmod = 6;
        foreach(GameObject Island in IslandList)
        {
            foreach (GameObject Island2 in IslandList)
            {
                if(Random.RandomRange(0,100) <= 1 && num < ChainNum)
                {
                    Pos1 = new Vector3(Island.transform.position.x, Island.transform.position.y, Island.transform.position.z);
                    Pos2 = new Vector3(Island2.transform.position.x, Island2.transform.position.y, Island2.transform.position.z);
                    GameObject Parent = new GameObject();
                    Parent.name = "Chain";
                    if (Pos1.x < Pos2.x && Pos1.y < Pos2.y)
                    {
                        Pos1.x += DIslands[Island.name.ToString()].x / posmod;
                        Pos1.z += DIslands[Island.name.ToString()].y / posmod;
                        Pos2.x += DIslands[Island2.name.ToString()].x / posmod;
                        Pos2.z += DIslands[Island2.name.ToString()].y / posmod;
                    }
                    else if (Pos1.x > Pos2.x && Pos1.y < Pos2.y)
                    {
                        Pos1.x -= DIslands[Island.name.ToString()].x / posmod;
                        Pos1.z += DIslands[Island.name.ToString()].y / posmod;
                        Pos2.x -= DIslands[Island2.name.ToString()].x / posmod;
                        Pos2.z += DIslands[Island2.name.ToString()].y / posmod;
                    }
                    else if (Pos1.x < Pos2.x && Pos1.y > Pos2.y)
                    {
                        Pos1.x += DIslands[Island.name.ToString()].x / posmod;
                        Pos1.z -= DIslands[Island.name.ToString()].y / posmod;
                        Pos2.x += DIslands[Island2.name.ToString()].x / posmod;
                        Pos2.z -= DIslands[Island2.name.ToString()].y / posmod;
                    }
                    else
                    {
                        Pos1.x -= DIslands[Island.name.ToString()].x / posmod;
                        Pos1.z -= DIslands[Island.name.ToString()].y / posmod;
                        Pos2.x -= DIslands[Island2.name.ToString()].x / posmod;
                        Pos2.z -= DIslands[Island2.name.ToString()].y / posmod;
                    }
                    ChainScript Chain = new ChainScript(Pos1, Pos2, Parent, Chainlink, FloatingRock);
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

        for (int x = 0; x < DIslands["IslandBasic"].x; x++)
        {
            for (int y = 0; y < DIslands["IslandBasic"].y; y++)
            {
                EIslandArray[x + 2000, y + 2000] = (int)EIslands.Filled;
            }
        }
        gameObjectIslands[2000, 2000] = GameObject.Instantiate(StartingIsland);
        gameObjectIslands[2000, 2000].transform.position = new Vector3(2000, Random.Range(100, RandomHeightLimit), 2000);
        gameObjectIslands[2000, 2000].transform.SetParent(transform);
        Island.Add(10);
        Location.Add(gameObjectIslands[2000, 2000].transform.position);

        for (int x = 0; x < 1000; x++)
        {
            for (int y = 0; y < 1000; y++)
            {
                EIslandArray[x + (int)(Width/2), y + (int)(Width / 2)] = (int)EIslands.Filled;
            }
        }
        gameObjectIslands[2000, 2000] = GameObject.Instantiate(BossIsland);
        gameObjectIslands[2000, 2000].transform.position = new Vector3((float)(Width/2.3), RandomHeightLimit + RandomHeightLimit/3, (float)(Length / 2.3));
        gameObjectIslands[2000, 2000].transform.SetParent(transform);
        Island.Add(11);
        Location.Add(gameObjectIslands[2000, 2000].transform.position);

        Vector2 ShopPos = new Vector2(Random.Range(0, Width), Random.Range(0, Length));
        for (int x = 0; x < DIslands["IslandBasic"].x; x++)
        {
            for (int y = 0; y < DIslands["IslandBasic"].y; y++)
            {
                EIslandArray[x + (int)ShopPos.x, y + (int)ShopPos.y] = (int)EIslands.Filled;
            }
        }
        gameObjectIslands[(int)ShopPos.x, (int)ShopPos.y] = GameObject.Instantiate(ShopIsland);
        gameObjectIslands[(int)ShopPos.x, (int)ShopPos.y].transform.position = new Vector3((int)ShopPos.x, Random.Range(100, RandomHeightLimit), (int)ShopPos.y);
        gameObjectIslands[(int)ShopPos.x, (int)ShopPos.y].transform.SetParent(transform);
        Island.Add(12);
        Location.Add(gameObjectIslands[(int)ShopPos.x, (int)ShopPos.y].transform.position);

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
                    Island.Add(IslRand);
                    Location.Add(gameObjectIslands[(int)pos.x, (int)pos.y].transform.position);
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
    private void LoadIsland(int num, Vector3 position)
    {
        GameObject temp = null;
        if(num >=0 && num < Islands.Count)
        {
            temp = Islands[num];
        }
        else if(num == 10)
        {
            temp = StartingIsland;
        }
        else if (num == 11)
        {
            temp = BossIsland;
        }
        else if (num == 12)
        {
            temp = ShopIsland;
        }
        Instantiate(temp, position, new Quaternion(0, 0, 0, 0));
    }
    public void LoadData(GameData data)
    {
        foreach (int num in data.Islands)
        {
            Island.Add(num);
        }
        foreach (Vector3 num in data.Location)
        {
            Location.Add(num);
        }
    }

    public void SaveData(ref GameData data)
    {
        //data.Islands = new List<int>();
        //data.Location = new List<Vector3>();
        data.Islands = new List<int>();
        foreach(int num in Island)
        {
            data.Islands.Add(num);
        }
        data.Location = new List<Vector3>();
        foreach (Vector3 num in Location)
        {
            data.Location.Add(num);
        }
    }

    public void RemoveIsland(Vector3 position)
    {
        for (int i = 0; i < Location.Count; i++){
            //Debug.Log("Island: " + Location[i] + "  +"+ position);
            if (Location[i].x == position.x)
            {
                Location.RemoveAt(i);
                Island.RemoveAt(i);
                Debug.Log("Total Island: " + Island.Count);
                break;
            }
        }
    }
}
