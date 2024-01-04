using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    private Dictionary<MenuType, MenuController> menuControllers = new Dictionary<MenuType, MenuController>();
    
    protected override void Awake()
    {
        base.Awake();
        AddMenusToDictionary();

        LevelSelector levelSelector = FindObjectOfType<LevelSelector>(true);
        if (levelSelector != null) levelSelector.Initialize();

        if(GameManager.Instance != null) GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void AddMenusToDictionary()
    {
        menuControllers.Clear();

        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in canvases)
        {
            MenuController[] controllers = canvas.GetComponentsInChildren<MenuController>(true);

            foreach(MenuController menuController in controllers)
            {
                menuControllers.Add(menuController.Type, menuController);
            }
        }
    }

    public MenuController FindMenu(MenuType menuTypeToFind)
    {
        return menuControllers.TryGetValue(menuTypeToFind, out var result) ? result : null;
    }

    private void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Initialization:
                if(!FindMenu(MenuType.GameplayMenu).gameObject.activeSelf) FindMenu(MenuType.GameOverMenu)?.gameObject.SetActive(true);
                break;
            case GameState.Paused:
                //FindMenu(MenuType.PauseMenu)?.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                FindMenu(MenuType.GameOverMenu)?.gameObject.SetActive(true);
                break;
            case GameState.Victory:
                FindMenu(MenuType.VictoryMenu)?.gameObject.SetActive(true);
                break;
        }
    }
}
