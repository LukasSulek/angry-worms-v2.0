using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class DifficultyController : MonoBehaviour
{
    public Dictionary<WormType, WormData> WormDatas { get; set; } = new Dictionary<WormType, WormData>();
    public List<LevelWormData> LevelWormDatas { get; set; } = new List<LevelWormData>();
    public List<WormCountIncreaseData> WormCountIncreaseDatas { get; set; } = new List<WormCountIncreaseData>();

    public delegate List<T> DataProvider<T>();
    public event DataProvider<LevelWormData> OnLevelWormDataRequested;
    public event DataProvider<WormCountIncreaseData> OnWormCountIncreaseDataRequested;
    public event DataProvider<SpeedIncreaseData> OnSpeedIncreaseDataRequested;

    public event Action<Vector2, Vector2> OnSpawnParametersChanged;

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    public void Initialize(LevelData levelData)
    {
        SpawnConfiguration spawnConfiguration = levelData.SpawnConfiguration;

        SubscribeToEvents(spawnConfiguration);

        List<LevelWormData> levelWormDatas = OnLevelWormDataRequested?.Invoke();
        LevelWormDatas.Add(new LevelWormData(levelWormDatas[0]));
        CalculateNewChancesT(levelWormDatas, LevelWormDatas);

        List<WormCountIncreaseData> wormCountIncreaseDatas = OnWormCountIncreaseDataRequested?.Invoke();
        WormCountIncreaseDatas.Add(new WormCountIncreaseData(wormCountIncreaseDatas[0]));
        CalculateNewChancesT(wormCountIncreaseDatas, WormCountIncreaseDatas);
    }

    private List<WormCountIncreaseData> ProvideWormCountIncreaseData()
    {
        return WormCountIncreaseDatas;
    }
    private List<LevelWormData> ProvideLevelWormData()
    {
        return LevelWormDatas;
    }
    private WormData ProvideWormData(WormType wormType)
    {
        if (WormDatas.ContainsKey(wormType)) return WormDatas?[wormType];
        else
        {
            Debug.Log("WormType not found");
            return null;
        }
    }

    private void IncreaseDifficulty(int tresholdIndex)
    {
        List<LevelWormData> levelWormDatas = OnLevelWormDataRequested?.Invoke();
        List<LevelWormData> wormDataList = GetItemsWithIndexT(tresholdIndex, levelWormDatas);
        if (wormDataList.Count > 0)
        {
            foreach (LevelWormData data in wormDataList)
            {
                if(!LevelWormDatas.Contains(data)) LevelWormDatas.Add(new LevelWormData(data));
            }

            CalculateNewChancesT(levelWormDatas, LevelWormDatas);
        }

        List<WormCountIncreaseData> wormCountIncreaseDatas = OnWormCountIncreaseDataRequested?.Invoke();
        List<WormCountIncreaseData> countDataList = GetItemsWithIndexT(tresholdIndex, wormCountIncreaseDatas);
        if (countDataList.Count > 0)
        {
            foreach (WormCountIncreaseData data in countDataList)
            {
                WormCountIncreaseDatas.Add(new WormCountIncreaseData(data));
            }

            CalculateNewChancesT(wormCountIncreaseDatas, WormCountIncreaseDatas);
        }

        List<Vector2> newSpawnSpeeds = UpdateSpawnSpeed(tresholdIndex);
        OnSpawnParametersChanged?.Invoke(newSpawnSpeeds[0], newSpawnSpeeds[1]);
    }
    private List<TItem> GetItemsWithIndexT<TItem>(int tresholdIndex, List<TItem> sourceList) where TItem : Data
    {
        List<TItem> newList = new List<TItem>();

        if (sourceList.Count == 0) return newList;

        foreach (TItem data in sourceList)
        {
            if (tresholdIndex == data.TresholdIndex && !newList.Contains(data)) newList.Add(data);
        }

        return newList;
    }
    private void CalculateNewChancesT<T>(List<T> dataList, List<T> list) where T : Data
    {
        float totalChance = 0;
        for(int i = 0; i < list.Count; i++)
        {
            totalChance += dataList[i].Chance;
        }
        if (totalChance == 0) return;

        for (int i = 0; i < list.Count; i++)
        {
            float newChance = 100 / totalChance * dataList[i].Chance;
            list[i].SetChance(newChance / 100);
        }
    }

    private List<Vector2> UpdateSpawnSpeed(int tresholdIndex)
    {
        SpeedIncreaseData matchingSpeedData = OnSpeedIncreaseDataRequested?.Invoke().FirstOrDefault(data => data.TresholdIndex == tresholdIndex);
        List<Vector2> list = new List<Vector2>();

        if (matchingSpeedData != null)
        {
            list.Add(matchingSpeedData.SpawnTime);
            list.Add(matchingSpeedData.SpawnDelay);
        }

        return list;
    }

    private void SubscribeToEvents(SpawnConfiguration spawnConfiguration)
    {
        OnLevelWormDataRequested += spawnConfiguration.ProvideLevelWormDatas;
        OnWormCountIncreaseDataRequested += spawnConfiguration.ProvideWormCountIncreaseDatas;
        OnSpeedIncreaseDataRequested += spawnConfiguration.ProvideSpeedIncreaseDatas;

        WormGenerator wormGenerator = GetComponent<WormGenerator>();
        if (wormGenerator != null)
        {
            wormGenerator.OnLevelWormDataRequested += ProvideLevelWormData;
            wormGenerator.OnWormCountIncreaseDataRequested += ProvideWormCountIncreaseData;
            wormGenerator.OnWormDataRequested += ProvideWormData;
        }

        TresholdManager tresholdManager = FindObjectOfType<TresholdManager>();
        if (tresholdManager != null) tresholdManager.OnTresholdReached += IncreaseDifficulty;
    }
    private void UnsubscribeFromEvents()
    {
        SpawnConfiguration spawnConfiguration = GameManager.Instance?.LevelDataAsset?.SpawnConfiguration;
        if (spawnConfiguration != null)
        {
            OnLevelWormDataRequested -= spawnConfiguration.ProvideLevelWormDatas;
            OnWormCountIncreaseDataRequested -= spawnConfiguration.ProvideWormCountIncreaseDatas;
            OnSpeedIncreaseDataRequested -= spawnConfiguration.ProvideSpeedIncreaseDatas;
        }

        WormGenerator wormGenerator = GetComponent<WormGenerator>();
        if (wormGenerator != null)
        {
            wormGenerator.OnLevelWormDataRequested -= ProvideLevelWormData;
            wormGenerator.OnWormCountIncreaseDataRequested -= ProvideWormCountIncreaseData;
            wormGenerator.OnWormDataRequested -= ProvideWormData;
        }

        TresholdManager tresholdManager = FindObjectOfType<TresholdManager>();
        if (tresholdManager != null) tresholdManager.OnTresholdReached -= IncreaseDifficulty;
    }
}
