using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public GameObject audioObject;

    public void StartAudio()
    {
        Instantiate(audioObject, transform.position, transform.rotation);
    }

}
