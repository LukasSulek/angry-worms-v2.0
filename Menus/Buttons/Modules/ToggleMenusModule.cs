using System.Collections.Generic;
using UnityEngine;

public class ToggleMenusModule : ButtonModule
{
    [SerializeField] private List<MenuType> menusToToggle;

    protected override void HandleButtonAction()
    {
        base.HandleButtonAction();

        ToggleMenus();
    }

    private void ToggleMenus()
    {
        foreach(MenuType menu in menusToToggle)
        {
            MenuController menuController = MenuManager.Instance.FindMenu(menu);

            menuController?.gameObject.SetActive(!menuController.gameObject.activeInHierarchy);
        }
    }


}
