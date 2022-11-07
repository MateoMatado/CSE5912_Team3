using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;

    private List<IData> dataObjects;
    public static DataManager Instance { get; private set; }
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one data");
        }
        Instance = this;
    }
    public void Start()
    {
        this.fileDataHandler = new FileDataHandler("Assets/Scripts/Save&Load/Data", fileName);
        this.dataObjects = FindAllData();
        NewGame();
    }
    public void NewGame()
    {
        this.gameData = fileDataHandler.NewGame();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data found");
            NewGame();
        }

        foreach (IData data in dataObjects)
        {
            data.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IData data in dataObjects)
        {
            data.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }
    private List<IData> FindAllData()
    {
        IEnumerable<IData> dataObject = FindObjectsOfType<MonoBehaviour>().OfType<IData>();
        return new List<IData>(dataObject);
    }
}
