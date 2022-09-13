using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitSceneControl : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject QuiButton;
    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(QuiButton);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        if(transform.position.y > 760)
        {
            Exit();
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
