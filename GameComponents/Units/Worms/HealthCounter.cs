using UnityEngine;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    private TextMeshProUGUI healthText { get; set; }
    public void Init(int value)
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        SetHealthText(value);
    }
    public void SetHealthText(int value)
    {
        if(healthText != null) healthText.text = value.ToString();
    }
}
