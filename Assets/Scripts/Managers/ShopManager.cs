
using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<ShopItemData> availableItems;

    public void Initialize()
    {
        LoadShopItems();
        Debug.Log("ShopManager initialized.");
    }

    void LoadShopItems()
    {
        // TODO: JSON 또는 ScriptableObject로부터 상점 아이템 데이터를 불러오는 로직 구현
        availableItems = new List<ShopItemData>();

        // 예시 아이템 추가
        availableItems.Add(new ShopItemData("고급 마이크", "방송 품질을 향상시키는 마이크", ItemType.Equipment, 10000,
            new List<ItemEffect> { new ItemEffect { parameterName = "talkSkill", value = 5 } }));
        availableItems.Add(new ShopItemData("새로운 의상", "매력을 증가시키는 의상", ItemType.Costume, 5000,
            new List<ItemEffect> { new ItemEffect { parameterName = "charm", value = 3 } }));
        availableItems.Add(new ShopItemData("피로 회복제", "스트레스를 즉시 감소시키는 약", ItemType.Consumable, 1000,
            new List<ItemEffect> { new ItemEffect { parameterName = "stress", value = -20 } }));
    }

    public bool BuyItem(ShopItemData item)
    {
        PlayerData playerData = GameManager.Instance.CharacterManager.CurrentPlayerData;

        if (playerData.money >= item.price)
        {
            GameManager.Instance.CharacterManager.AddMoney(-item.price);
            ApplyItemEffects(item);
            Debug.Log($"Purchased {item.itemName} for {item.price} money.");
            return true;
        }
        else
        {
            Debug.Log("Not enough money to buy this item.");
            return false;
        }
    }

    void ApplyItemEffects(ShopItemData item)
    {
        PlayerData playerData = GameManager.Instance.CharacterManager.CurrentPlayerData;
        if (playerData == null) return;

        foreach (var effect in item.effects)
        {
            switch (effect.parameterName)
            {
                case "talkSkill":
                    playerData.talkSkill += effect.value;
                    break;
                case "charm":
                    playerData.charm += effect.value;
                    break;
                case "stress":
                    playerData.stress = Mathf.Clamp(playerData.stress + effect.value, 0, 100);
                    break;
                // 다른 파라미터들에 대한 처리 추가
            }
        }
    }
}
