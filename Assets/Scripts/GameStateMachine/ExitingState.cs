using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitingState : GameState
{
    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("Exit Scene");
    }
}
