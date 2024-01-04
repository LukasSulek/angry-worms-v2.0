using UnityEngine;
using UnityEngine.UI;
using System;

public class GrassGenerator : MonoBehaviour, IInitializer
{
    private class SpawnRanges
    {
        public Vector2 minPosition { get; set; }
        public Vector2 maxPosition { get; set; }

        public SpawnRanges(Vector2 minPosition, Vector2 maxPosition)
        {
            this.minPosition = minPosition;
            this.maxPosition = maxPosition;
        }
    }

    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Vector2Int spawnAmount = new Vector2Int(12, 20);
    [SerializeField] private int offset = 60;
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Initialize(LevelData levelData)
    {
        SpawnRanges[] spawnRanges = CalculateSpawnRanges();
        
        GenerateGrass(spawnRanges);
    }

    private void GenerateGrass(SpawnRanges[] spawnRanges)
    {
        int grassCount = UnityEngine.Random.Range(Math.Abs(spawnAmount.x), Math.Abs(spawnAmount.y));

        for (int i = 0; i < grassCount; i++)
        {
            Vector2 position = ChoosePosition(spawnRanges);
            GameObject grass = Utilities.GenerateObject("Grass", grassSprite, position, transform);
        }
    }

    private Vector2 ChoosePosition(SpawnRanges[] spawnRanges)
    {
        int index = UnityEngine.Random.Range(0, spawnRanges.Length);
        SpawnRanges spawnRange = spawnRanges[index];

        float x = UnityEngine.Random.Range(spawnRange.minPosition.x, spawnRange.maxPosition.x);
        float y = UnityEngine.Random.Range(spawnRange.minPosition.y, spawnRange.maxPosition.y);
        Vector2 position = new Vector2(x, y);

        return position;
    }

    private SpawnRanges[] CalculateSpawnRanges()
    {
        SpawnRanges[] spawnRanges = new SpawnRanges[2];

        Vector2 bottomMinPosition = new Vector2(offset, offset);
        Vector2 bottomMaxPosition = new Vector2(Screen.width - offset, Screen.height * 0.265625f - 135 - offset);

        Vector2 topMinPosition = new Vector2(offset, Screen.height * 0.265625f + 3.5f * 270 + offset);
        Vector2 topMaxPosition = new Vector2(Screen.width - offset, Screen.height - offset);

        SpawnRanges bottomRange = new SpawnRanges(bottomMinPosition, bottomMaxPosition);
        SpawnRanges topRange = new SpawnRanges(topMinPosition, topMaxPosition);

        spawnRanges[0] = bottomRange;
        spawnRanges[1] = topRange;

        return spawnRanges;
    }

}
