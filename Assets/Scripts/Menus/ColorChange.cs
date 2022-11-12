using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorChange : MonoBehaviour
{
    public Image image;

    // Update is called once per frame
    public void Selected()
    {
        image.color = new Color(236 / 255f, 209 / 255f, 61 / 255f);        
    }

    public void Normal()
    {
        image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
    }
}
