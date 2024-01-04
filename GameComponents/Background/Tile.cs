using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 Position {get; set;}
    public TileStatus Status {get; set;}

    public void Init(Vector2 position, TileStatus status)
    {
        Position = position;
        Status = status;
    }
}
