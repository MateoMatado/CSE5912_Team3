using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningState : GameState
{
    public override void Enter()
    {
        base.Enter();
        string gameScene = "DemoScene";
        if (SceneManager.GetActiveScene().name != gameScene)
        {
            SceneManager.LoadScene(gameScene);
        }
    }
}
