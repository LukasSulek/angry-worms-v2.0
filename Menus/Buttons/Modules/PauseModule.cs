using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseModule : ButtonModule
{
    protected override void HandleButtonAction()
    {
        base.HandleButtonAction();

        
        Pause();
    }

    private void Pause()
    {
        switch(GameManager.Instance.CurrentGameState)
        {
            case GameState.Gameplay:
                GameManager.Instance.ChangeGameState(GameState.Paused);
                break;
            case GameState.Paused:
                GameManager.Instance.ChangeGameState(GameState.Gameplay);
                break;
        }
    }

}
