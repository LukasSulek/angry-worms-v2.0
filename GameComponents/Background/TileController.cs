using System.Collections.Generic;
using UnityEngine;
using System;

public enum LayoutType { Tile, None }
public enum TileStatus { Empty, Full }

public class TileController : MonoBehaviour, IInitializer
{
    [SerializeField] private Sprite tileSprite;

    public Dictionary<int, Tile> Tiles = new Dictionary<int, Tile>();
    public Dictionary<int, Tile> EmptyTiles { get; set; } = new Dictionary<int, Tile>();

    public event Action<int> OnTileRemoved;
    public event Action<int> OnTileAdded;

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

        GenerateTiles(levelData.LayoutConfiguration.Positions);
    }

    private List<int> ProvideEmptyTileIndices()
    {
        List<int> emptyTileIndices = new List<int>(EmptyTiles.Keys);
        return emptyTileIndices;
    }
    private void RemoveTile(int index)
    {
        EmptyTiles.Remove(index);
        OnTileRemoved?.Invoke(index);
    }
    private void AddTile(int index)
    {
        EmptyTiles.Add(index, Tiles[index]);
        OnTileAdded?.Invoke(index);
    }
    private void GenerateTiles(List<Vector2> positions)
    {
        for(int i = 0; i < positions.Count; i++)
        {
            Tile tile = Utilities.GenerateObject("Tile", tileSprite, positions[i], transform).AddComponent<Tile>();
            tile.Init(positions[i], TileStatus.Empty);
            Tiles.Add(i, tile);
            AddTile(i);
        }
    } 

    private void SubscribeToEvents()
    {
        WormGenerator wormGenerator = FindObjectOfType<WormGenerator>();
        if (wormGenerator != null)
        {
            wormGenerator.OnEmptyTileIndicesRequested += ProvideEmptyTileIndices;
            wormGenerator.OnWormGenerated += RemoveTile;
        }

        VegetableGenerator vegetableGenerator = FindObjectOfType<VegetableGenerator>();
        if (vegetableGenerator != null) vegetableGenerator.OnVegetableGrown += AddTile;
    }
    private void UnsubscribeFromEvents()
    {
        WormGenerator wormGenerator = FindObjectOfType<WormGenerator>();
        if (wormGenerator != null)
        {
            wormGenerator.OnEmptyTileIndicesRequested -= ProvideEmptyTileIndices;
            wormGenerator.OnWormGenerated -= RemoveTile;
        }

        VegetableGenerator vegetableGenerator = FindObjectOfType<VegetableGenerator>();
        if(vegetableGenerator != null) vegetableGenerator.OnVegetableGrown -= AddTile;
    }
}
