using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class ItemDefinition : ScriptableObject
{
    public int id;
    public string displayName;
    public string description;
}
