using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerHP : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Slider slider;
    //[SerializeField] PlayerStatus playerStatus;

    private void Start()
    {
        slider.maxValue = 1000;
        slider.value = 1000;
    }

    private void Update()
    {
        Debug.Log("PLAYER HP in UI: " + playerHealth.currentHealth );
        //slider.value = 500;
        //playerStatus.HealthChange();
        slider.value = playerHealth.currentHealth;
    }

}
