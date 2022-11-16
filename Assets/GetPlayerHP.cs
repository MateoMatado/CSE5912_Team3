using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerHP : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Slider slider;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1000;
        slider.value = 1000;
    }

    private void Update()
    {
        Debug.Log("PLAYER HP in UI: " + playerHealth.currentHealth );
        slider.value = 500;
        //slider.value = playerHealth.currentHealth;
    }

}
