using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    private GameData gameData;
    private FileSave fileSave;
    private GameInput inputs;
    private List<IData> dataObjects;
    public static GameDataManager Instance { get; private set; }
    private FileDataHandler fileDataHandler;
    /*file Name*/

    private bool canEnter = false;

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
        fileSave = LoadFile();
        if (this.fileSave == null)
        {
            this.fileSave = new FileSave();
        }

        
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSave.fileRead);
        this.dataObjects = FindAllData();
        LoadGame();
    }



    public void NewGame()
    {
        /**
         * 
         * foreach (IData data in dataObjects)
        {
            data.NewData(ref gameData);
        }
        fileDataHandler.Save(gameData);
         */
        this.gameData = new GameData();

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

    public FileSave LoadFile()
    {
        //create direcotry path
        string fullPath = Path.Combine(Application.persistentDataPath, "fileSaved");
        //string fullPath = Path.Combine("Assets/Scripts/Save&Load/Data", "fileSaved");
        FileSave loadedFile = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load Data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedFile = JsonUtility.FromJson<FileSave>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file" + fullPath + "\n" + e);
            }
        }
        return loadedFile;
    }
}
