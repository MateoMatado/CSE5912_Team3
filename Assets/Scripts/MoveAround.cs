using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] bool move;
    [SerializeField] Vector3 worldMove;
    [SerializeField] Vector3 localMove;
    [SerializeField] float moveSpeed;

    [Header("Rotation")]
    [SerializeField] bool rotate;
    [SerializeField] Vector3 rotation;
    [SerializeField] float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position += Time.deltaTime * worldMove * moveSpeed;
            Vector3 diff = Time.deltaTime * moveSpeed * localMove;
            transform.position += diff.x * transform.right;
            transform.position += diff.y * transform.up;
            transform.position += diff.z * transform.forward;
        }

        if (rotate)
        {
            Vector3 angles = transform.rotation.eulerAngles;
            angles += rotation * rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(angles);
        }
    }
}
