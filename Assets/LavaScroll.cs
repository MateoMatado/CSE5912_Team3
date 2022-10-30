using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.0001f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(-offset, offset));
    }
}

