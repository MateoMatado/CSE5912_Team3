using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimations : MonoBehaviour
{
    [SerializeField] Boolean Rotate = false;
    [SerializeField] Vector3 RotateDirection = new Vector3(0,0,0);
    [SerializeField] Boolean Float = false;
    [SerializeField] float FloatHeight = 0;
    [SerializeField] float FloatSpeed = 0;

    private Boolean FloatSwitch = false;
    private Vector3 InitPos;
    void Start()
    {
        InitPos = transform.position;
    }
    void Update()
    {
        if (Rotate)
        {
           RotateObject();
        }
        if (Float)
        {
            FloatObject();
        }

    }
    private void RotateObject()
    {
        gameObject.transform.Rotate(RotateDirection);
    }
    private void FloatObject()
    {
        if (FloatSwitch)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, FloatSpeed, 0);
        }
        else
        {
            gameObject.transform.position = gameObject.transform.position - new Vector3(0, FloatSpeed, 0);
        }
        if(gameObject.transform.position.y > FloatHeight + InitPos.y) { FloatSwitch = false;}
        else if(gameObject.transform.position.y < InitPos.y){ FloatSwitch = true;}
    }
}
