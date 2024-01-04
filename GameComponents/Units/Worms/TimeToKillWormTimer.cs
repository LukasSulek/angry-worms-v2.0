using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeToKillWormTimer: MonoBehaviour
{
    private Image Image { get; set; }
    private float startTime { get; set; }
    public void Init(float value, float maxValue)
    {
        if(Image == null) Image = GetComponentInChildren<Image>();
        SetValue(value, maxValue);
    }
    public void SetValue(float value, float maxValue)
    {
        Image.fillAmount = value / maxValue;
    }
}
