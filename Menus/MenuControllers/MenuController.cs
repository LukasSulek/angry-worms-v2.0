using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MenuType type;   
    public MenuType Type => type; 

    protected Dictionary<ButtonType, ButtonController> buttonControllers = new Dictionary<ButtonType, ButtonController>();

    protected void Awake()
    {
        AddButtonsToList();
    }

    public virtual void AddButtonsToList()
    {
        buttonControllers.Clear();

        ButtonController[] buttons = GetComponentsInChildren<ButtonController>();

        foreach(ButtonController buttonController in buttons)
        {
            if(buttonController.Type != ButtonType.LevelButton)
            {
                buttonControllers.Add(buttonController.Type, buttonController);
            }
        }
    }

}
