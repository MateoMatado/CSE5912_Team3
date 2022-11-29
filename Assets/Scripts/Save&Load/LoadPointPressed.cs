using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public class LoadPointPressed : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text fileName;
    public GameObject ResetButton;
    public GameObject Loadpoint1;
    public GameObject Loadpoint2;
    public GameObject Loadpoint3;
    public GameObject Loadpoint4;
    public GameObject chosen;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ResetButton.SetActive(true);
            Loadpoint1.SetActive(false);
            Loadpoint2.SetActive(false);
            Loadpoint3.SetActive(false);
            Loadpoint4.SetActive(false);
            chosen.SetActive(true);
        }
    }
    public void Back()
    {
        Loadpoint1.SetActive(true);
        Loadpoint2.SetActive(true);
        Loadpoint3.SetActive(true);
        Loadpoint4.SetActive(true);
    }
    public void Pressed1()
    {
        DataManager.Instance.Pressed(fileName,1);
    }

    public void Pressed2()
    {
        DataManager.Instance.Pressed(fileName, 2);
    }
    public void Pressed3()
    {
        DataManager.Instance.Pressed(fileName, 3);
    }
    public void Pressed4()
    {
        DataManager.Instance.Pressed(fileName, 4);
    }

    public void Reset1()
    {
        fileName.text = "New Data";
        DataManager.Instance.Reset(1);
        Back();
    }
    public void Reset2()
    {
        fileName.text = "New Data";
        DataManager.Instance.Reset(2);
        Back();
    }
    public void Reset3()
    {
        fileName.text = "New Data";
        DataManager.Instance.Reset(3);
        Back();
    }
    public void Reset4()
    {
        fileName.text = "New Data";
        DataManager.Instance.Reset(4);
        Back();
    }

    public void DeleteFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName.text);
        File.Delete(fullPath);
#if UNITY_EDITOR
         UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
