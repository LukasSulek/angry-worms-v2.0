using UnityEngine;

[System.Serializable]
public class LevelWormData : Data
{
    [SerializeField] private WormType wormType;
    public WormType WormType => wormType;

    public LevelWormData(LevelWormData data)
    {
        this.wormType = data.WormType;
        this.chance = data.Chance;
        this.tresholdIndex = data.TresholdIndex;
    }
}
