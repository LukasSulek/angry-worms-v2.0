using UnityEngine;

[CreateAssetMenu(fileName = "NewVegetableData", menuName = "ScriptableData/Vegetable")]
public class VegetableData : ScriptableData
{
    [SerializeField] private VegetableType vegetableType;
    public VegetableType VegetableType => vegetableType;
}
