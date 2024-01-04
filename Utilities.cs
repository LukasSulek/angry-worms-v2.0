using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Utilities
{
    public static float[] CalculateCumulativeChancesT<T>(List<T> dataList) where T : Data
    {
        if (dataList.Count == 0 || dataList == null) return null;

        float[] cumulativeChances = new float[dataList.Count];
        float totalChance = 0f;

        for (int i = 0; i < dataList.Count; i++)
        {
            totalChance += dataList[i].Chance;
            cumulativeChances[i] = totalChance;
        }

        return cumulativeChances;
    }
    public static T GetRandomDataT<T>(List<T> dataList, float[] cumulativeChances) where T : Data
    {
        if (dataList == null || dataList.Count == 0) return default(T);
        if (cumulativeChances == null || cumulativeChances.Length == 0) return null;

        float randomValue = Random.Range(0f, cumulativeChances[cumulativeChances.Length - 1]);

        for (int i = 0; i < cumulativeChances.Length; i++)
        {
            if (randomValue <= cumulativeChances[i])
            {  
                return dataList[i];   
            }
        }

        return dataList[0];
    }
    public static GameObject GenerateObject(string name, Sprite sprite, Vector2 position, Transform parent)
    {
        GameObject newObject = new GameObject(name);
        Image image = newObject.AddComponent<Image>();
        if (sprite != null) image.sprite = sprite;
        newObject.transform.SetParent(parent);

        RectTransform rectTransform = newObject.GetComponent<RectTransform>();
        if(sprite != null) rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
        rectTransform.position = position;

        return newObject;
    }
}
