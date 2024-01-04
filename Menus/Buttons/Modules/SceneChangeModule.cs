using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneChangeModule : ButtonModule
{
    [SerializeField] private SceneType scene;
    [SerializeField] private string dataKey;
    public string DataKey
    {
        get { return dataKey; }
        set { dataKey = value; }
    }

    protected void Start()
    {
        if (GameManager.Instance != null) dataKey = GameManager.Instance.LevelDataAsset.name.ToString();
    }

    protected override void HandleButtonAction()
    {
        base.HandleButtonAction();

        ChangeScene();
    }

    private void ChangeScene()
    {
        switch (scene)
        {
            case SceneType.GameScene:
                DataLoadSystem.Instance.LoadScene(scene, dataKey);
                break;
            case SceneType.MainScene:
                DataLoadSystem.Instance.LoadScene(scene);
                break;
        }
    }
}
