using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TresholdManager : MonoBehaviour, IInitializer
{
    public int CurrentTresholdKey { get; set; } = 0;
    public int NextTresholdKey { get; set; } = 1;

    private float timeToNextTreshold = 0;
    public event Action<int> OnTresholdReached;

    private Dictionary<int, float> tresholds;
    
    private Coroutine tresholdTimerCoroutine = null;

    public void Initialize(LevelData levelData)
    {
        if(GameManager.Instance != null) GameManager.Instance.OnGameStateChange += OnGameStateChange;
        tresholds = new Dictionary<int, float>(levelData.TresholdsConfiguration.Init());
        OnTresholdReached += CalculateNextTreshold;

        if(tresholdTimerCoroutine == null) tresholdTimerCoroutine = StartCoroutine(TimeToNextTreshold());
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null) GameManager.Instance.OnGameStateChange -= OnGameStateChange;

        if (tresholdTimerCoroutine != null) StopCoroutine(tresholdTimerCoroutine);
        tresholdTimerCoroutine = null;
    }

    public void CalculateNextTreshold(int tresholdIndex)
    {
        if(tresholdTimerCoroutine != null) StopCoroutine(tresholdTimerCoroutine);
        tresholdTimerCoroutine = null;

        if (CurrentTresholdKey == tresholds.Count) return;

        if (tresholdTimerCoroutine == null) tresholdTimerCoroutine = StartCoroutine(TimeToNextTreshold());
    }

    private IEnumerator TimeToNextTreshold()
    {
        if (CurrentTresholdKey == 0) timeToNextTreshold = tresholds[NextTresholdKey];
        else timeToNextTreshold = tresholds[NextTresholdKey] - tresholds[CurrentTresholdKey];

        while (timeToNextTreshold >= 0)
        {
            timeToNextTreshold -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        CurrentTresholdKey++;
        NextTresholdKey = CurrentTresholdKey + 1;

        OnTresholdReached?.Invoke(CurrentTresholdKey);
    }
    private void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
            case GameState.GameOver:
            case GameState.Victory:
                if (tresholdTimerCoroutine != null) StopCoroutine(tresholdTimerCoroutine);
                tresholdTimerCoroutine = null;
                break;
        }
    }
}
