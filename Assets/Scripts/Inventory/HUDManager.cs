using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider HP;
    [SerializeField] private Slider Mana;


    // Update is called once per frame
    void Update()
    {
        HP.value = PlayerStatus.Instance.GetValue("HP") / 100;
        Mana.value = PlayerStatus.Instance.GetValue("Mana") / 100;
    }
}
