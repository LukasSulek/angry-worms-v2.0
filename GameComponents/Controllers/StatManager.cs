using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StatManager : MonoBehaviour, IInitializer
{
    public int WormsKilled { get; set; } = 0;
    public int Score { get; set; }
    public float PlayTime { get; set; } = 0;

    private const float deltaTime = 0.01f;
    private Coroutine timerCoroutine;
    public event Action<int> OnScoreChanged;

    private void OnDestroy()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnGameStateChange -= OnGameStateChange;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Initialize(LevelData levelData)
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }
    public void OnWormKilled(WormBehaviour worm)
    {
        WormsKilled++;

        Score += worm.ScoreValue;
        OnScoreChanged?.Invoke(Score);
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(deltaTime);
            PlayTime += deltaTime;
        }
    }
    private void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                if(timerCoroutine == null) timerCoroutine = StartCoroutine(Timer());
                OnScoreChanged?.Invoke(0);
                break;
            case GameState.Paused:
            case GameState.GameOver:
            case GameState.Victory:
                if (timerCoroutine != null) StopCoroutine(timerCoroutine);
                timerCoroutine = null;
                break;
        }
    }
}
