
using UnityEngine;
using System.Collections.Generic;

public enum ItemType
{
    None,
    Consumable,
    Equipment,
    Costume
}

[System.Serializable]
public class ItemEffect
{
    public string parameterName; // PlayerData의 파라미터 이름
    public int value;            // 변화량
}

[System.Serializable]
public class ShopItemData
{
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public long price;
    public List<ItemEffect> effects;

    public ShopItemData(string name, string description, ItemType type, long price, List<ItemEffect> effects)
    {
        this.itemName = name;
        this.itemDescription = description;
        this.itemType = type;
        this.price = price;
        this.effects = effects;
    }
}
