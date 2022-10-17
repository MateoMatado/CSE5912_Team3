using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider HP;
    [SerializeField] private Slider Mana;
    public static HUDManager Instance;
    public GameObject CollectPanel;
    public Text number;
    private int coin = 0;
    public void Awake()
    {
        Instance = this;
    }
    public void DisplayCollectPanel(string name)
    {
        var text = CollectPanel.transform.Find("ItemName").GetComponent<Text>();
        text.text = "Press E to collect " + name;
        CollectPanel.SetActive(true);
    }
    public void CloseCollectPanel()
    {
        CollectPanel.SetActive(false);
    }
    public void GetCoin(int number)
    {
        coin += number;
    }
    // Update is called once per frame
    void Update()
    {
        HP.value = PlayerStatus.Instance.GetValue("HP") / 100;
        Mana.value = PlayerStatus.Instance.GetValue("Mana") / 100;
        number.text = coin + "";
    }
}
