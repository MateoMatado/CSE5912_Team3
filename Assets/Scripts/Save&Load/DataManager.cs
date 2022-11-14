using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class DataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    private GameData gameData;
    private GameInput inputs;
    private List<IData> dataObjects;
    public static DataManager Instance { get; private set; }
    private FileDataHandler fileDataHandler;
    /*file Name*/
    public TMP_Text fileName;
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
            canEnter = false;
            fileName.text = InputFileName.text;
            InputField.SetActive(false);
            this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName.text);
            this.dataObjects = FindAllData();
            LoadGame();
            GameStateMachine.Instance.SwitchState(GameStateMachine.RunningState);
        }

    }
    public void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, "test");
        this.dataObjects = FindAllData();
        LoadGame();
    }

    public void Pressed()
    {
        if (fileName.text.Equals("New Data"))
        {
            InputField.SetActive(true);
            canEnter = true;
        }
    }

    public void NewGame()
    {
        foreach (IData data in dataObjects)
        {
            data.NewData(ref gameData);
        }
        fileDataHandler.Save(gameData);
        

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
