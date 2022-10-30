using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    [SerializeField] LensFlare flare;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(flare.transform.position, cam.transform.position) > 500)
        {
            flare.enabled = false;
        }
        else
        {
            flare.enabled = true;
        }
    }
}
