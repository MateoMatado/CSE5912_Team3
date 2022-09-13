using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardStartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel, optionsMenuPanel, newGamePopOut;
    [SerializeField] private GameObject newGameBtn, loadGameBtn, optionsBtn;
    [SerializeField] private GameObject optionsFirstBtn, optionsClosedBtn;
    [SerializeField] private GameObject saveFirstBtn, loadFirstBtn;
    [SerializeField] private GameObject graphicsFirstBtn, soundFirstBtn;




    public void OnClickNewGame()
    {
        EventSystem.current.SetSelectedGameObject(saveFirstBtn);
    }
    public void OnClickNewGameAnswerNo()
    {
        EventSystem.current.SetSelectedGameObject(newGameBtn);
    }

    public void OnClickLoadGame()
    {
        EventSystem.current.SetSelectedGameObject(loadFirstBtn);
    }
    public void OnClickLoadAnswerNo()
    {
        EventSystem.current.SetSelectedGameObject(loadGameBtn);
    }

    public void OnClickLoadAnswerYes()
    {
        // Temporary code.
        // will continue on after finishing Save/Load feature
        EventSystem.current.SetSelectedGameObject(loadGameBtn);
    }

    public void OnClickOptions()
    {
        EventSystem.current.SetSelectedGameObject(optionsFirstBtn);
    }
    public void OnClickOptionsAnswerNo()
    {
        EventSystem.current.SetSelectedGameObject(optionsBtn);
    }

    public void OnClickGraphics()
    {
        EventSystem.current.SetSelectedGameObject(graphicsFirstBtn);
    }
    public void OnClickGraphicsBack()
    {
        EventSystem.current.SetSelectedGameObject(optionsFirstBtn);
    }
    public void OnClickSound()
    {
        EventSystem.current.SetSelectedGameObject(soundFirstBtn);
    }
    public void OnClickSoundBack()
    {
        EventSystem.current.SetSelectedGameObject(optionsFirstBtn);
    }










    /*
    public void OpenNewGamePopOut()
    {
        newGamePopOut.SetActive(true);
    }
    public void CloseNewGamePopOut()
    {
        newGamePopOut.SetActive(false);
    }
    */
    
}
