using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseFirstBtn, SettingFirstBtn;
    // Start is called before the first frame update
    public void OnClickSetting()
    {
        EventSystem.current.SetSelectedGameObject(SettingFirstBtn);
    }
}
