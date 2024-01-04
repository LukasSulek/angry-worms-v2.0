using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelButtonController : ButtonController
{
    public TextMeshProUGUI LevelText {get; set;}

    public GameObject LockImage {get; set;}
    [SerializeField] private Sprite lockSprite;

    public int LevelIndex {get; set;}
    public LevelStatus Status {get; set;}

    public void Init(int levelIndex, int maxReachedLevel)
    {
        LevelIndex = levelIndex;
        LevelText.text = levelIndex.ToString();

        if(LevelIndex <= maxReachedLevel)
        {
            Status = LevelStatus.Unlocked;
            LevelText.gameObject.SetActive(true);

            if(LockImage != null) Destroy(LockImage.gameObject);
        }
        else
        {
            Status = LevelStatus.Locked;
            LevelText.gameObject.SetActive(false);
            CreateLock();
        }

        GetComponent<SceneChangeModule>().DataKey = "Level" + levelIndex + "Data";
    }

    private void CreateLock()
    {
        LockImage = new GameObject("LockImage");
        LockImage.AddComponent<Image>().sprite = lockSprite;
        LockImage.GetComponent<RectTransform>().sizeDelta = new Vector2(167, 168);
        LockImage.transform.SetParent(transform);  
    }

    protected override void OnClick()
    {
        switch(Status)
        {
            case LevelStatus.Locked:
                Debug.Log("Level locked");
                //Audio
                break;
            case LevelStatus.Unlocked:
                //Audio
                base.OnClick();
                break;
        }
    }


}
