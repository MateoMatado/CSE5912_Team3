using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        GameStateMachine.Instance.SwitchState(new LoadState());
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(GameStateMachine.Instance.CurrentState);
        }
    }
}
