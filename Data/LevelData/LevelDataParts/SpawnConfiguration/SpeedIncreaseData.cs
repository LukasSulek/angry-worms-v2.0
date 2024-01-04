using UnityEngine;

[System.Serializable]
public class SpeedIncreaseData
{
    [SerializeField] private int tresholdIndex;
    public int TresholdIndex => tresholdIndex;

    [SerializeField] private Vector2 spawnTime;
    public Vector2 SpawnTime => spawnTime;

    [SerializeField] private Vector2 spawnDelay;
    public Vector2 SpawnDelay => spawnDelay;
}
