using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ButtonType type;
    public ButtonType Type => type;
    private List<ButtonModule> buttonModules = new List<ButtonModule>();

    private void Awake()
    {
        ButtonModule[] modules = GetComponents<ButtonModule>();

        foreach(ButtonModule module in modules)
        {
            buttonModules.Add(module);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    protected virtual void OnClick()
    {
        buttonModules.ForEach(module => module?.Execute());
    } 

}
