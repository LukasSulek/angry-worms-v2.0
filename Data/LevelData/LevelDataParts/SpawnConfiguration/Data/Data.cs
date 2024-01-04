using UnityEngine;
public class Data
{
    [SerializeField] protected float chance;
    public float Chance => chance;

    public void SetChance(float value)
    {
        this.chance = value;
    }

    [SerializeField] protected int tresholdIndex;
    public int TresholdIndex => tresholdIndex;
}
