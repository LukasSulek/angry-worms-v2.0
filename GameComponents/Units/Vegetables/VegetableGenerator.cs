using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class VegetableGenerator : Generator, IInitializer
{
    [SerializeField] private const float vegetableGrowDelay = 0.5f;
    public List<VegetableData> VegetableDatas { get; set; } = new List<VegetableData>();
    public Dictionary<int, VegetableBehaviour> Vegetables { get; set; } = new Dictionary<int, VegetableBehaviour>();
    public event Action<int> OnVegetableGrown;

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Initialize(LevelData levelData)
    {
        if(levelData != null) InitializeVegetables(levelData);
    }
    private void InitializeVegetables(LevelData levelData)
    {
        InitializeObjectsT<VegetableData, VegetableBehaviour>(levelData, Vegetables);
        foreach (KeyValuePair<int, VegetableBehaviour> vegetable in Vegetables)
        {
            vegetable.Value.Init();
            vegetable.Value.ChangeDataT(ChooseData());
        }
    }
    private VegetableData ChooseData()
    {
        if (VegetableDatas.Count > 0)
        {
            VegetableData data = VegetableDatas?[UnityEngine.Random.Range(0, VegetableDatas.Count)];
            return data;
        }
        else return null;
    }

    public void OnVegetableEaten(int index)
    {
        if (Vegetables.ContainsKey(index))
        {
            Vegetables[index].gameObject.SetActive(false);
        }
    }

    public void OnWormKilled(WormBehaviour wormKilled)
    {
        StartCoroutine(GrowVegetable(wormKilled.Index));
    }
    private IEnumerator GrowVegetable(int index)
    {
        yield return new WaitForSeconds(vegetableGrowDelay);

        if (VegetableDatas.Count > 0)
        {
            Vegetables[index].ChangeDataT(ChooseData());
        }
        
        OnVegetableGrown?.Invoke(index);
    }
}
