using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private readonly Dictionary<int, int> items = new Dictionary<int, int>();
    private readonly Dictionary<int, ItemDefinition> definitions = new Dictionary<int, ItemDefinition>();

    void Awake()
    {
        LoadDefinitions();
    }

    void LoadDefinitions()
    {
        ItemDefinition[] defs = Resources.LoadAll<ItemDefinition>("Items");
        foreach (var def in defs)
            definitions[def.id] = def;
    }

    public void AddItem(int id, int amount = 1)
    {
        if (!definitions.ContainsKey(id))
        {
            Debug.LogWarning($"Item id {id} not found");
            return;
        }
        if (!items.ContainsKey(id))
            items[id] = 0;
        items[id] += amount;
    }

    public bool RemoveItem(int id, int amount = 1)
    {
        if (!items.ContainsKey(id) || items[id] < amount)
            return false;
        items[id] -= amount;
        if (items[id] <= 0)
            items.Remove(id);
        return true;
    }

    public int GetItemCount(int id)
    {
        return items.TryGetValue(id, out var count) ? count : 0;
    }

    public ItemDefinition GetItemDefinition(int id)
    {
        definitions.TryGetValue(id, out var def);
        return def;
    }
}
