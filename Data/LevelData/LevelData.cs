using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableData/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int levelDataIndex;
    public int LevelDataIndex => levelDataIndex;

    [SerializeField] private LayoutConfiguration layoutConfiguration = null;
    public LayoutConfiguration LayoutConfiguration => layoutConfiguration;

    [SerializeField] private SpawnConfiguration spawnConfiguration = null;
    public SpawnConfiguration SpawnConfiguration => spawnConfiguration;

    [SerializeField] private TresholdsConfiguration tresholdsConfiguration = null;
    public TresholdsConfiguration TresholdsConfiguration => tresholdsConfiguration;
}
