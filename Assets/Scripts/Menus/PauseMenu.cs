using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Team3.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject PauseFirstBtn, SettingFirstBtn;
        [SerializeField] private GameObject settingMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private AudioSource BGM;
        [SerializeField] private Slider BGMVolume;
        public static bool played = true;
        void Start()
        {
            EventSystem.current.SetSelectedGameObject(PauseFirstBtn);
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        public void Save()
        {
            DataManager.Instance.SaveGame();
        }

        public void Setting()
        {
            settingMenu.SetActive(true);
            pauseMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(SettingFirstBtn);
        }

        public void Back()
        {
            settingMenu.SetActive(false);
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(PauseFirstBtn);
        }

        public void Exit()
        {
            GameStateMachine.Instance.SwitchState(GameStateMachine.MainMenuState);
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
}