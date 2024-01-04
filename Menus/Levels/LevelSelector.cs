using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum LevelStatus
{
    Locked, Unlocked
}

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int totalNumberOfLevels;
    public int MaxReachedLevel { get; set; } = 1;
    private RectTransform levelContent;

    [SerializeField] private LevelButtonController levelButtonPrefab;

    [Header("Pool settings")]
    [SerializeField] private int maxButtonPoolAmount = 6;
    private List<LevelButtonController> buttonPool = new List<LevelButtonController>();


    public void Initialize()
    {
        levelContent = GetComponentInChildren<GridLayoutGroup>(true).GetComponent<RectTransform>();

        for(int i = 1; i <= maxButtonPoolAmount; i++)
        {
            LevelButtonController levelButton = CreateLevelButton();
            buttonPool.Add(levelButton);
            levelButton.Init(i, MaxReachedLevel);
            levelButton.gameObject.SetActive(false);
        }
    }

    private LevelButtonController CreateLevelButton()
    {
        LevelButtonController levelButton = Instantiate(levelButtonPrefab, levelButtonPrefab.transform.position, Quaternion.identity, levelContent.transform);

        
        levelButton.LevelText = levelButton.GetComponentInChildren<TextMeshProUGUI>();
        return levelButton;
    }
}
