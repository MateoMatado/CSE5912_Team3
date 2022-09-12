using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private Slider BGMVolume;
    public static bool played = true;
    public void Save()
    {
        //TODO: after having save
    }

    public void Setting()
    {
        settingMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        settingMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Exit()
    {
        GameStateMachine.Instance.SwitchState(new MainMenuState());
    }

    public void BGMControl()
    {
        if (played)
        {
            BGM.Stop();
            played = false;
        }
        else
        {
            played = true;
            BGM.Play();
        }
    }

    public void Update()
    {
        BGM.volume = BGMVolume.value;
    }
}
