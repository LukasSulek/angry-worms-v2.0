using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour, IInitializer
{
    private TextMeshProUGUI scoreText;
    private void OnDestroy()
    {
        StatManager statManager = FindObjectOfType<StatManager>();
        if(statManager != null) statManager.OnScoreChanged -= UpdateScoreText;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void Initialize(LevelData levelData)
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        StatManager statManager = FindObjectOfType<StatManager>();
        if (statManager != null) statManager.OnScoreChanged += UpdateScoreText;
    }
    private void UpdateScoreText(int scoreValue)
    {
        if(scoreText != null) scoreText.text = scoreValue.ToString();
    }
}
