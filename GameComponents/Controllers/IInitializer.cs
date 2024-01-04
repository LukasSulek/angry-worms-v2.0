using UnityEngine;
public interface IInitializer
{
    GameObject GetGameObject();
    void Initialize(LevelData levelData);
}
