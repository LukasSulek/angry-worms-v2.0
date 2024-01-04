using UnityEngine;

[System.Serializable]
public class WormCountIncreaseData : Data
{
    [SerializeField] private int wormCount;
    public int WormCount => wormCount;
    [SerializeField] private float limitMultiplier;
    public float LimitMultiplier => limitMultiplier;

    public WormCountIncreaseData(WormCountIncreaseData data)
    {
        this.wormCount = data.WormCount;
        this.limitMultiplier = data.LimitMultiplier;
        this.chance = data.Chance;
        this.tresholdIndex = data.TresholdIndex;
    }   

}