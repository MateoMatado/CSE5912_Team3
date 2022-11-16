using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using TMPro;


public class LoadPointPressed : MonoBehaviour
{
    public TMP_Text fileName;
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
}
