using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;
using System.IO;

public class DataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    private GameData gameData;
    private FileSave fileSave;
    private GameInput inputs;
    private List<IData> dataObjects;
    public static DataManager Instance { get; private set; }
    private FileDataHandler fileDataHandler;
    /*file Name*/
    private int number = 0;
    public TMP_Text fileName1;
    public TMP_Text fileName2;
    public TMP_Text fileName3;
    public TMP_Text fileName4;
    public GameObject InputField;
    public TMP_InputField InputFileName;
    private bool canEnter = false;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        inputs = new GameInput();
        inputs.UI.Yes.performed += Yes_performed;
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
        SaveFile(fileSave);
        fileName1.text = fileSave.file1;
        fileName2.text = fileSave.file2;
        fileName3.text = fileSave.file3;
        fileName4.text = fileSave.file4;
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSave.fileRead);
        this.dataObjects = FindAllData();
        LoadGame();
    }
    private void OnEnable()
    {
        inputs.UI.Yes.Enable();
    }
    private void OnDisable()
    {
        inputs.UI.Yes.Enable();
    }
    private void Yes_performed(InputAction.CallbackContext context)
    {
        if (canEnter)
        {
            CreateGame();
        }

    }

    public void CreateGame()
    {
        canEnter = false;
        InputField.SetActive(false);
        UpdateFile(ref fileSave, InputFileName.text, number);
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSave.fileRead);
        this.dataObjects = FindAllData();
        LoadGame();
        GameStateMachine.Instance.SwitchState(GameStateMachine.RunningState);
    }

    public void Pressed(TMP_Text fileName, int num)
    {
        number = num;
        if (fileName.text.Equals("New Data"))
        {
            InputField.SetActive(true);
            canEnter = true;
        }
        else
        {
            fileSave.fileRead = fileName.text;
            GameStateMachine.Instance.SwitchState(GameStateMachine.RunningState);
        }
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

    public void SaveFile(FileSave data)
    {
        //create direcotry path
        string fullPath = Path.Combine(Application.persistentDataPath, "fileSaved");
        //string fullPath = Path.Combine("Assets/Scripts/Save&Load/Data", "fileSaved");
        try
        {
            //create direcotry
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //change data into Json
            string dataToStore = JsonUtility.ToJson(data, true);
            //write data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file" + fullPath + "\n" + e);
        }
    }

    public void UpdateFile(ref FileSave fileSave, string fileName, int num)
    {
        switch (num)
        {
            case 1:
                fileSave.file1 = InputFileName.text;
                break;
            case 2:
                fileSave.file2 = InputFileName.text;
                break;
            case 3:
                fileSave.file3 = InputFileName.text;
                break;
            case 4:
                fileSave.file4 = InputFileName.text;
                break;
        }
        fileSave.fileRead = InputFileName.text;
        SaveFile(fileSave);
    }
}
