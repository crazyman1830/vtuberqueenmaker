
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform itemContainer;
    public ShopItemUI shopItemUIPrefab;

    private List<ShopItemUI> currentShopItems = new List<ShopItemUI>();

    public void Initialize()
    {
        shopPanel.SetActive(false);
    }

    public void ShowShop()
    {
        shopPanel.SetActive(true);
        RefreshShopItems();
    }

    public void HideShop()
    {
        shopPanel.SetActive(false);
    }

    void RefreshShopItems()
    {
        // 기존 아이템 UI 제거
        foreach (var itemUI in currentShopItems)
        {
            Destroy(itemUI.gameObject);
        }
        currentShopItems.Clear();

        // 새로운 아이템 UI 생성 및 추가
        foreach (var itemData in GameManager.Instance.ShopManager.availableItems)
        {
            ShopItemUI newItemUI = Instantiate(shopItemUIPrefab, itemContainer);
            newItemUI.Initialize(itemData, OnBuyButtonClicked);
            currentShopItems.Add(newItemUI);
        }
    }

    void OnBuyButtonClicked(ShopItemData itemData)
    {
        if (GameManager.Instance.ShopManager.BuyItem(itemData))
        {
            // 구매 성공 시 UI 업데이트
            RefreshShopItems();
            GameManager.Instance.UIGameManager.UpdatePlayerStatsUI(GameManager.Instance.CharacterManager.CurrentPlayerData);
        }
    }
}
