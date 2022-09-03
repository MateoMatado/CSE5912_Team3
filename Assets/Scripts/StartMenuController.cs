using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuController : MonoBehaviour
{    
    string levelToLoad;
    [SerializeField] private GameObject unableToLoad = null;
    [SerializeField] private string newGameLevel = "Ball&Plane";
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    public void NewGameYes()
    {
        SceneManager.LoadScene(newGameLevel);
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
        Application.Quit();
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
