using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfiguration
{
    [SerializeField] private Vector2 startSpawnTime;
    public Vector2 StartSpawnTime => startSpawnTime;
    [SerializeField] private Vector2 startSpawnDelay;
    public Vector2 StartSpawnDelay => startSpawnDelay;

    [SerializeField] private List<SpeedIncreaseData> speedIncreaseDatas;
    public List<SpeedIncreaseData> ProvideSpeedIncreaseDatas()
    {
        return speedIncreaseDatas;
    }

    [SerializeField] private List<VegetableType> vegetableTypes;
    public List<VegetableType> VegetableTypes => vegetableTypes;

    [SerializeField] private List<LevelWormData> levelWormDatas;
    public List<LevelWormData> ProvideLevelWormDatas()
    {
        return levelWormDatas;
    }

    [SerializeField] private List<WormCountIncreaseData> wormCountIncreaseDatas;
    public List<WormCountIncreaseData> ProvideWormCountIncreaseDatas()
    {
        return wormCountIncreaseDatas;
    }
}

