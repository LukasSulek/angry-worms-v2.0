using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WormGenerator : Generator, IInitializer
{
    public delegate List<T> DataProvider<T>();
    public event DataProvider<int> OnEmptyTileIndicesRequested;
    public event DataProvider<LevelWormData> OnLevelWormDataRequested;
    public event DataProvider<WormCountIncreaseData> OnWormCountIncreaseDataRequested;

    public delegate WormData WormDataProvider(WormType wormType);
    public event WormDataProvider OnWormDataRequested;

    public event Action OnSpawnInterval;
    public event Action<GameState> OnMaximumWormsReached;
    public event Action<int> OnWormGenerated;

    public int ActiveWorms { get; private set; } = 0;
    public int MaximumWorms { get; private set; } = 16;
    public Vector2 SpawnTime { get; private set; } = new Vector2(1f, 1f);
    public Vector2 SpawnDelay { get; private set; } = new Vector2(0f, 0.3f);
    [SerializeField] private const float startDelay = 2f;

    public Dictionary<int, WormBehaviour> Worms { get; private set; } = new Dictionary<int, WormBehaviour>();

    private bool gameStarted = false;
    private Coroutine generatorCoroutine = null;

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Initialize(LevelData levelData)
    {
        SubscribeToEvents();

        MaximumWorms = levelData.LayoutConfiguration.Positions.Count;

        DifficultyController difficultyController = GetComponent<DifficultyController>();
        if (difficultyController != null)
        {
            difficultyController.OnSpawnParametersChanged += UpdateSpawnParameters;
            difficultyController.Initialize(levelData);
        }

        SpawnConfiguration spawnConfiguration = levelData.SpawnConfiguration;
        SpawnTime = spawnConfiguration.StartSpawnTime;
        SpawnDelay = spawnConfiguration.StartSpawnDelay;

        InitializeWorms(levelData);
    }
    private void InitializeWorms(LevelData levelData)
    {
        VegetableGenerator vegetableGenerator = FindObjectOfType<VegetableGenerator>();
        StatManager statManager = FindObjectOfType<StatManager>();

        InitializeObjectsT<WormData, WormBehaviour>(levelData, Worms);
        foreach (KeyValuePair<int, WormBehaviour> worm in Worms)
        {
            worm.Value.Init();
            worm.Value.OnWormKilled += DecrementActiveWorms;
            OnWormGenerated += worm.Value.EatVegetable;

            worm.Value.OnWormKilled += vegetableGenerator.OnWormKilled;
            worm.Value.OnVegetableEaten += vegetableGenerator.OnVegetableEaten;

            worm.Value.OnWormKilled += statManager.OnWormKilled;
        }
    }
    private void DecrementActiveWorms(WormBehaviour worm)
    {
        if(ActiveWorms > 0) ActiveWorms--;
    }
    private void IncrementActiveWorms(int index)
    {
        if(ActiveWorms < MaximumWorms) ActiveWorms++;
        if(ActiveWorms >= MaximumWorms) OnMaximumWormsReached?.Invoke(GameState.GameOver);
    }

    private void UpdateSpawnParameters(Vector2 newSpawnTime, Vector2 newSpawnDelay)
    {
        SpawnTime = newSpawnTime;
        SpawnDelay = newSpawnDelay;
    }

    private IEnumerator Generator()
    {
        if (!gameStarted)
        {
            yield return new WaitForSeconds(startDelay);
            gameStarted = true;
        }

        while (true)
        {
            int wormsToSpawn = CalculateWormsToSpawn();

            for(int i = 0; i < wormsToSpawn; i++)
            {
                OnSpawnInterval?.Invoke();

                yield return new WaitForSeconds(UnityEngine.Random.Range(SpawnDelay.x, SpawnDelay.y));
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(SpawnTime.x, SpawnTime.y));
        }
    }
    private void GenerateWorm()
    {
        List<int> emptyTileIndices = OnEmptyTileIndicesRequested?.Invoke();

        if (emptyTileIndices.Count <= 0) return;
        int index = emptyTileIndices[UnityEngine.Random.Range(0, emptyTileIndices.Count)];

        WormData data = ChooseData();
        Worms[index].ChangeDataT(data);

        OnWormGenerated?.Invoke(index);
    }
    public WormData ChooseData()
    {
        List<LevelWormData> levelWormDatas = OnLevelWormDataRequested?.Invoke();
        float[] cumulativeChances = Utilities.CalculateCumulativeChancesT(levelWormDatas);
        LevelWormData data = Utilities.GetRandomDataT(levelWormDatas, cumulativeChances);

        WormData wormData = OnWormDataRequested?.Invoke(data.WormType);

        return wormData;
    }
    private int CalculateWormsToSpawn()
    {
        if (ActiveWorms >= MaximumWorms) return 0;

        List<WormCountIncreaseData> wormCountIncreaseDatas = OnWormCountIncreaseDataRequested?.Invoke();
        float[] cumulativeChances = Utilities.CalculateCumulativeChancesT(wormCountIncreaseDatas);
        WormCountIncreaseData data = Utilities.GetRandomDataT(wormCountIncreaseDatas, cumulativeChances);

        for (int i = wormCountIncreaseDatas.IndexOf(data); i > 0; i--)
        {
            if (ActiveWorms <= MaximumWorms * wormCountIncreaseDatas[i].LimitMultiplier) return data.WormCount;
            else data = wormCountIncreaseDatas[i -1];
        }

        return data.WormCount;
    }

    private void OnGameStateChange(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Gameplay:
                if(generatorCoroutine == null) generatorCoroutine = StartCoroutine(Generator());
                break;
            case GameState.Paused:
            case GameState.GameOver:
            case GameState.Victory:
                if(generatorCoroutine != null) StopCoroutine(generatorCoroutine);
                generatorCoroutine = null;

                foreach(KeyValuePair<int, WormBehaviour> worm in Worms)
                {
                    worm.Value.ResetTimerCoroutine();
                }

                break;
        }
    }
    private void SubscribeToEvents()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnGameStateChange += OnGameStateChange;

        OnSpawnInterval += GenerateWorm;
        OnMaximumWormsReached += GameManager.Instance.ChangeGameState;
        OnWormGenerated += IncrementActiveWorms;
    }
    private void UnsubscribeFromEvents()
    {
        if(GameManager.Instance != null) GameManager.Instance.OnGameStateChange -= OnGameStateChange;

        OnSpawnInterval -= GenerateWorm;
        if(GameManager.Instance != null) OnMaximumWormsReached -= GameManager.Instance.ChangeGameState;
        OnWormGenerated -= IncrementActiveWorms;
    }
}
