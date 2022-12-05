using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depth = Camera.main.depth - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
