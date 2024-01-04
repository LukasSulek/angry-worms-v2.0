using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitBehaviour : MonoBehaviour, IPointerClickHandler
{
    public Image Image {get; set;}
    public RectTransform RectTransform {get; set;}
    public T LoadDataT<T>(ScriptableData newData) where T : ScriptableData
    {
        T data = (T)newData;
        return data;
    }
    public virtual void ChangeDataT<T>(T data) where T : ScriptableData
    {
        T newData = LoadDataT<T>(data);

        if(newData != null)
        {
            gameObject.name = newData.ObjectName;
            Image.sprite = newData.Sprite;
            RectTransform.sizeDelta = new Vector2(newData.Size.x, newData.Size.y);

            if(!gameObject.activeSelf) gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(this + " - ChangeDataT - newData is null");
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnInteraction();
    }
    protected virtual void OnInteraction() {}
    public virtual void Init() { }

}
