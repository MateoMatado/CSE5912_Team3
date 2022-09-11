using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject pauseMenu;
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
}
