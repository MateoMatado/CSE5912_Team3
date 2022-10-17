using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCollisionHandler : MonoBehaviour
{
    private void Awake()
    {
        Physics.IgnoreLayerCollision(0, 7);
    }
}
