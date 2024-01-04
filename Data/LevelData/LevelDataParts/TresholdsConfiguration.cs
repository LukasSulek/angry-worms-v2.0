using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TresholdsConfiguration
{
    [SerializeField] private int numberOfTresholds;
    [SerializeField] private Vector2 difference = new Vector2();
    [SerializeField] private float multiplier = 1;
    [SerializeField] private bool createRandomTresholds = false;

    [SerializeField] private List<float> values;

    public Dictionary<int, float> Init()
    {
        Dictionary<int, float> tresholds = new Dictionary<int, float>();

        if(createRandomTresholds || values.Count <= 0) CreateRandomTresholdsValues();

        for(int i = 1; i < values.Count + 1; i++)
        {
            tresholds.Add(i, values[i - 1]);
        }

        return tresholds;
    }
    private void CreateRandomTresholdsValues()
    {
        values = new List<float>();
        
        for(int i = 0; i < numberOfTresholds; i++)
        {
            if (i == 0)
            {
                values.Add(Random.Range(difference.x, difference.y) * multiplier);
            }
            else
            {
                float previousValue = values[i - 1];
                float newValue = previousValue + Random.Range(difference.x, difference.y) * multiplier;
                values.Add(newValue);
            }
        }
    }
}
