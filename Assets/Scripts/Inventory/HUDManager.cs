using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour, IData
{
    [SerializeField] private Slider HP;
    [SerializeField] private Slider Mana;
    public static HUDManager Instance;
    public GameObject CollectPanel;
    public Text number;
    private int coin = 500;
    public float hp = 1000;
    public Vector3 position;
    public void Awake()
    {
        Instance = this;
        coin = 500;
        position = new Vector3(0,0,0);
    }
    public void DisplayCollectPanel(string name, int price)
    {
        var text = CollectPanel.transform.Find("ItemName").GetComponent<Text>();
        text.text = "Press E to collect " + name + " ($" + price +")";
        if (AbleToBuy(price))
        {
            text.color = Color.white;
        }
        else
        {
            text.color = Color.red;
        }
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
    public bool AbleToBuy(int price)
    {
        if(price > coin)
        {
            return false;
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        //Jimmy Commented Out
        //HP.value = PlayerStatus.Instance.GetValue("HP") / 100;


        Mana.value = PlayerStatus.Instance.GetValue("Mana") / 100;
        number.text = coin + "";
    }

    public void LoadData(GameData data)
    {
        coin = data.Coin;
        hp = data.HP;
        position = data.playerPosition;
        //PlayerStatus.Instance.Mana = data.MP;
    }

    public void SaveData(ref GameData data)
    {
        data.Coin = coin;
        data.HP = PlayerHealth.Instance.GetHP();
        data.playerPosition = PlayerStatus.Instance.position();
        //data.MP = PlayerStatus.Instance.Mana;
    }
}
