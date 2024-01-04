using UnityEngine;

[CreateAssetMenu(fileName = "NewWormData", menuName = "ScriptableData/Worm")]
public class WormData : ScriptableData
{
    [SerializeField] private WormType wormType = WormType.Normal;
    public WormType WormType => wormType;
    
    [SerializeField] private int tapsToKill = 1;
    public int TapsToKill => tapsToKill;

    [SerializeField] private float timeToKill = 4f;
    public float TimeToKill => timeToKill;

    [SerializeField] private int scoreValue = 1;
    public int ScoreValue => scoreValue;

    [SerializeField] private HealthCounter healthCounter = null;
    public HealthCounter HealthCounter => healthCounter;

    [SerializeField] private TimeToKillWormTimer wormKillTimer = null;
    public TimeToKillWormTimer WormKillTimer => wormKillTimer;
}
