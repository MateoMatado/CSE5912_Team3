using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StartMenuController : MonoBehaviour
{    
    string levelToLoad;
    [SerializeField] private GameObject unableToLoad = null;
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;


    





    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }


    public void NewGameYes()
    {
        GameStateMachine.Instance.SwitchState(new RunningState());
    }
    public void LoadGameYes()
    {
        /*if (savedFileExist)
        {
          loadFile();
        }
        */
        unableToLoad.SetActive(true);
    }

    public void ExitYes()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void SetVolume(float volumeValue)
    {
        AudioListener.volume = volumeValue/100;
        volumeTextValue.text = ((int)volumeValue).ToString();
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

}
