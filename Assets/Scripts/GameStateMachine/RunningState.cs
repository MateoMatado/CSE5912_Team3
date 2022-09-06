using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningState : GameState
{
    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("Ball&Plane");
    }
}
