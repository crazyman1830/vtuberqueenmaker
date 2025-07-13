
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemDescriptionText;
    public Button buyButton;

    private ShopItemData currentItemData;
    private Action<ShopItemData> onBuyButtonClicked;

    public void Initialize(ShopItemData itemData, Action<ShopItemData> buyCallback)
    {
        currentItemData = itemData;
        onBuyButtonClicked = buyCallback;

        itemNameText.text = itemData.itemName;
        itemPriceText.text = $"{itemData.price.ToString("N0")}원";
        itemDescriptionText.text = itemData.itemDescription;

        buyButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        buyButton.onClick.AddListener(() => onBuyButtonClicked?.Invoke(currentItemData));
    }
}
