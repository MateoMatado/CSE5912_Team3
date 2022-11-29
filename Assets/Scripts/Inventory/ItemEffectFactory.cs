using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectFactory : MonoBehaviour
{
    public static ItemEffectFactory Instance;
    public GameObject effect;
    public ParticleSystem ps;
    public Light light;
    private bool CD = false;
    // Start is called before the first frame update
    public void Awake()
    {
        /*make it Instance*/
        Instance = this;

    }

    public void Effect(string name)
    {
        CD = true;
        switch (name)
        {
            case "Red Pot":
                PlayerHealth.Instance.HealthChange(50);
                ps.startColor = new Color(245/255f, 121 / 255f, 121 / 255f); //red
                light.color = new Color(245 / 255f, 121 / 255f, 121 / 255f);
                break;
            case "Blue Pot":
                PlayerStatus.Instance.ManaChange(50);
                ps.startColor = new Color(42 / 255f, 166 / 255f, 255 / 255f); //blue
                light.color = new Color(42 / 255f, 166 / 255f, 255 / 255f);
                break;
            case "Green Pot":
                PlayerStatus.Instance.SpeedChange(30f);
                ps.startColor = new Color(148 / 255f, 255 / 255f, 42 / 255f); //green
                light.color = new Color(148 / 255f, 255 / 255f, 42 / 255f);
                break;
            case "Bigger Pot":
                PlayerStatus.Instance.GetBigger(30f);
                ps.startColor = new Color(255 / 255f, 167 / 255f, 42 / 255f);//orange
                light.color = new Color(255 / 255f, 167 / 255f, 42 / 255f);
                break;
            case "Potions Red":
                PlayerHealth.Instance.HealthChange(20);
                ps.startColor = new Color(245 / 255f, 121 / 255f, 121 / 255f); //red
                light.color = new Color(245 / 255f, 121 / 255f, 121 / 255f);
                break;
            case "Potions Blue":
                PlayerStatus.Instance.ManaChange(20);
                ps.startColor = new Color(42 / 255f, 166 / 255f, 255 / 255f); //blue
                light.color = new Color(42 / 255f, 166 / 255f, 255 / 255f);
                break;
            case "Potions Green":
                PlayerStatus.Instance.SpeedChange(10f);
                ps.startColor = new Color(148 / 255f, 255 / 255f, 42 / 255f); //green
                light.color = new Color(148 / 255f, 255 / 255f, 42 / 255f);
                break;
        }
        StartCoroutine(PlayEffect());
    }
    IEnumerator PlayEffect()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        effect.SetActive(false);
        CD = false;
    }

    public bool InCD()
    {
        return CD;
    }
}
