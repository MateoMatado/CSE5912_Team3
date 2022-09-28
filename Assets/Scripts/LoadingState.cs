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
    [SerializeField] private GameObject CreditTo;
    [SerializeField] private GameObject picture1;
    [SerializeField] private GameObject picture2;
    [SerializeField] private GameObject picture3;
    [SerializeField] private float slideDuration = 1.5f;
    [SerializeField] private float loadDuration = 2f;
    public Slider slider;
    public Text ValueText;
    public Text LoadingText;
    private int count = 0;
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        /*Loading bar*/
        while(slider.value != 1)
        {
            /*update Message*/
            slider.value += 0.01f;
            ValueText.text = (int)(slider.value * 100f) + "%";
            count++;
            /*change text*/
            if (count == 20)
            {
                LoadingText.text = "Loading Map";
            }
            else if (count == 40)
            {
                LoadingText.text = "Loading Player";
            }
            else if (count == 60)
            {
                LoadingText.text = "Loading Music";
            }
            else if (count == 80)
            {
                LoadingText.text = "Loading Story";
            }
            yield return new WaitForSeconds(0.02f);
        }
       
        yield return new WaitForSeconds(0.5f);


        /*CSE Logo*/
        var cloneCreditTo = Instantiate(CreditTo, transform.position, transform.rotation);
        yield return new WaitForSeconds(slideDuration/2);
        Destroy(cloneCreditTo);

        /*Picture*/
        var Picture1 = Instantiate(picture1, transform.position, transform.rotation);
        yield return new WaitForSeconds(slideDuration);
        Destroy(Picture1);

        var Picture2 = Instantiate(picture2, transform.position, transform.rotation);
        yield return new WaitForSeconds(slideDuration);
        Destroy(Picture2);

        var Picture3 = Instantiate(picture3, transform.position, transform.rotation);
        yield return new WaitForSeconds(slideDuration);
        
        /*Switch to main menu*/
        GameStateMachine.Instance.SwitchState(GameStateMachine.MainMenuState);

    }
}
