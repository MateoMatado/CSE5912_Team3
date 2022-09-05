using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingState : MonoBehaviour
{
    [SerializeField] private GameObject Unity;
    [SerializeField] private GameObject OSU;
    [SerializeField] private GameObject CSE;
    [SerializeField] private GameObject picture1;
    [SerializeField] private GameObject picture2;
    [SerializeField] private GameObject picture3;
    public Slider slider;
    public Text ValueText;
    public Text LoadingText;
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        /*Loading bar*/
        slider.value = 0.2f;
        ValueText.text = slider.value * 100f + "%";
        LoadingText.text = "Loading Story";
        yield return new WaitForSeconds(0.2f);

        slider.value = 0.4f;
        ValueText.text = slider.value * 100f + "%";
        LoadingText.text = "Loading Characters";
        yield return new WaitForSeconds(0.2f);

        slider.value = 0.6f;
        ValueText.text = slider.value * 100f + "%";
        LoadingText.text = "Loading Weapons";
        yield return new WaitForSeconds(0.2f);

        slider.value = 0.8f;
        ValueText.text = slider.value * 100f + "%";
        LoadingText.text = "Loading Map";
        yield return new WaitForSeconds(0.2f);

        slider.value = 1f;
        ValueText.text = slider.value * 100f + "%";
        LoadingText.text = "Done";
        yield return new WaitForSeconds(0.2f);

        /*Unity Logo*/
        var cloneUnity =Instantiate(Unity, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(cloneUnity);

        /*OSU Logo*/
        var cloneOSU = Instantiate(OSU, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(cloneOSU);

        /*CSE Logo*/
        var cloneCSE = Instantiate(CSE, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(cloneOSU);

        /*Picture*/
        var Picture1 = Instantiate(picture1, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(Picture1);

        var Picture2 = Instantiate(picture2, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(Picture2);

        var Picture3 = Instantiate(picture3, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        
        /*Switch to main menu*/
        SceneManager.LoadScene(0);

    }
}
