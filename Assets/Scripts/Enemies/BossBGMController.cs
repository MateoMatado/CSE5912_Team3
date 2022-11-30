using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGMController : MonoBehaviour
{
    AudioSource bgmAudioSource;
    [SerializeField] AudioClip bossBGM;
    // Start is called before the first frame update
    void Start()
    {
        bgmAudioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        bgmAudioSource.clip = bossBGM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
