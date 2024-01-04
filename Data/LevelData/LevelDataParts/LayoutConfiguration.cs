using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LayoutConfiguration
{
    private const float startYMultiplier = 0.265625f;
    public int RowCount {get; set;} = 4;
    public int ColCount {get; set;} = 4;

    [SerializeField] private LayoutType[] layoutTypes = new LayoutType[16];
    public LayoutType[] LayoutTypes => layoutTypes;
    public List<Vector2> Positions { get; set; } 

    public void Init()
    {
       Positions = new List<Vector2>(CreateLayout());
    }

    private List<Vector2> CreateLayout()
    {
        List<Vector2> positions = new List<Vector2>();
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

        int width = screenSize.x / ColCount;
        int height = ColCount * width;
        int x = width / 2;
        float startY = screenSize.y * startYMultiplier;

        int index = 0;

        for (int i = 0; i < RowCount; i++)
        {
            float y = startY;

            for (int j = 0; j < ColCount; j++)
            {
                if (LayoutTypes[index] == LayoutType.Tile)
                {
                    Vector2 position = new Vector2(x, y);
                    positions.Add(position);
                }

                index++;
                y += width;
            }

            x += width;
        }

        return positions;
    }
}
