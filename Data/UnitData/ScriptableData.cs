using UnityEngine;

public class ScriptableData : ScriptableObject
{
    [SerializeField] private string objectName = "";
    public string ObjectName => objectName;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private Vector2 size = new Vector2(245, 246);
    public Vector2 Size => size;
}
