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
        slider.maxValue = 1000;
        slider.value = 1000;
    }

    
    private void Update()
    {
        //Debug.Log("GetPlayerHP.cs : " + playerHealth.currentHealth);
        //slider.value = playerHealth.currentHealth;

        Debug.Log("GetPlayerHP.cs : " + playerHealth.GetHP());
        slider.value = playerHealth.GetHP();
    }
    
}
