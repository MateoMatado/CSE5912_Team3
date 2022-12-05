using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLowerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depth = Camera.current.depth - 1;
    }
}
