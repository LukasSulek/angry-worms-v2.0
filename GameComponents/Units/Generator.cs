using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Generator : MonoBehaviour
{
    public void InitializeObjectsT<TData, TBehaviour>(LevelData levelData, Dictionary<int, TBehaviour> unitBehaviours) where TData : ScriptableData where TBehaviour : UnitBehaviour
    {
        unitBehaviours.Clear();

        for (int index = 0; index < levelData.LayoutConfiguration.Positions.Count; index++)
        {
            GameObject generatedObject = Utilities.GenerateObject("", null, levelData.LayoutConfiguration.Positions[index], transform);
            generatedObject.gameObject.AddComponent<InteractionEffect>();

            TBehaviour behaviour = generatedObject.AddComponent<TBehaviour>();

            behaviour.Image = generatedObject.GetComponent<Image>();
            behaviour.RectTransform = generatedObject.GetComponent<RectTransform>();
            if (behaviour is WormBehaviour wormBehaviour)
            {
                wormBehaviour.Index = index;
                generatedObject.gameObject.SetActive(false);
            }

            unitBehaviours.Add(index, behaviour);
        }
    }
}
