using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopLotBuyingListener : MonoBehaviour, IPointerClickHandler
{
    public event Action OnPlayerTryBuyShopLot;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPlayerTryBuyShopLot?.Invoke();
    }
}