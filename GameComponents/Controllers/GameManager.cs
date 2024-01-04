using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; }
    public event Action<GameState> OnGameStateChange;
    public LevelData LevelDataAsset { get; set; }

    public async void LoadLevelData(LevelData data)
    {
        LevelDataAsset = data;

        VegetableGenerator vegetableGenerator = FindObjectOfType<VegetableGenerator>();
        List<VegetableData> vegetableDatas = (List<VegetableData>)await DataLoadSystem.Instance.LoadDatasT<VegetableData>("VegetableData");

        if (vegetableDatas.Count > 0)
        {
            for(int i = 0; i < data.SpawnConfiguration.VegetableTypes.Count; i++)
            {
                foreach (VegetableData vegetableData in vegetableDatas)
                {
                    if (vegetableData.VegetableType == data.SpawnConfiguration.VegetableTypes[i])
                    {
                        vegetableGenerator.VegetableDatas.Add(vegetableDatas[i]);
                    }
                }
            }
        }
        if (LevelDataAsset != null) ChangeGameState(GameState.Initialization);
        else Debug.Log("Data not loaded - Game state not changed to initialization!");
    }

    public void ChangeGameState(GameState gameState)
    {
        if (gameState == CurrentGameState) return;
        CurrentGameState = gameState;

        HandleGameState(CurrentGameState);
        OnGameStateChange?.Invoke(CurrentGameState);
    }

    private void HandleGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Initialization:
                HandleInitialization();
                break;
            case GameState.Gameplay:
                HandleGameplay();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
        }
    }

    private void HandleInitialization()
    {
        LevelDataAsset.LayoutConfiguration.Init();

        List<IInitializer> initializers = FindObjectsOfType<MonoBehaviour>().OfType<IInitializer>().ToList();
        initializers.ForEach(initializer => initializer.Initialize(LevelDataAsset));

        ChangeGameState(GameState.Gameplay);
    }

    private void HandleGameplay()
    {
        
    }

    private void HandlePaused()
    {
        
    }

    private void HandleGameOver()
    {
        
    }

    private void HandleVictory()
    {
        
    }
}