using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    private float speed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        //float y = Mathf.PingPong(Time.time * speed, 0.5f) + 100;
        //transform.position = new Vector3(transform.position.x, y, transform.position.z);
        transform.Rotate(0, 0, 0.5f);
    }
}
