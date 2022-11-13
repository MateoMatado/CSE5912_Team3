using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTable : MonoBehaviour
{
    public GameObject Shop;
    public GameObject Hint;
    private bool isInRange = false;
    private GameInput inputs;

    public void Awake()
    {
        inputs = new GameInput();
        inputs.Player.Collect.performed += Collect_performed;
        
    }
    private void OnEnable()
    {
        inputs.Player.Collect.Enable();
    }
    private void OnDisable()
    {
        inputs.Player.Collect.Disable();
    }
    /*following three functions is to open inventory*/
    private void Collect_performed(InputAction.CallbackContext context)
    {
        if (isInRange )
        {
            Shop.SetActive(true);
            Hint.SetActive(false);
        }

    }
    void OnTriggerEnter(Collider col)
    {
        isInRange = true;
        Hint.SetActive(true);

    }

    void OnTriggerExit(Collider col)
    {
        isInRange = false;
        Hint.SetActive(false);
        Shop.SetActive(false);
    }
}
