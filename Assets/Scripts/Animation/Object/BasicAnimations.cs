using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimations : MonoBehaviour
{
    [SerializeField] Boolean Rotate = false;
    [SerializeField] Vector3 RotateDirection = new Vector3(0,0,0);
    [SerializeField] Boolean Float = false;
    [SerializeField] int FloatHeight = 0;
    [SerializeField] int FloatSpeed = 0;

    Boolean FloatSwitch = false;
    void Update()
    {
        if (Rotate)
        {
            gameObject.transform.Rotate(RotateDirection);
        }
        if (Float)
        {
            gameObject.transform.position = gameObject.transform.localPosition + new Vector3(0, FloatSpeed, 0);
        }

    }
}
