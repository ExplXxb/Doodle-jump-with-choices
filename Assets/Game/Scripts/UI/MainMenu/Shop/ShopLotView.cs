using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopLotView : MonoBehaviour
{
    [SerializeField] private ShopLotBuyingListener _shopLotBuyingListener;
    [SerializeField] private Image _shopLotIconSprite;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Transform _chargesContainer;
    [SerializeField] private ChargeUI _chargePrefab;

    public event Action OnBuyClicked;

    private readonly List<ChargeUI> _pool = new();

    private void Awake()
    {
        _shopLotBuyingListener.OnPlayerTryBuyShopLot += () => OnBuyClicked?.Invoke();
    }

    public void Render(Sprite icon, bool iconIsActive, string costText, int currentLevel, int maxLevel)
    {
        _shopLotIconSprite.sprite = icon;
        _costText.text = costText;

        if (iconIsActive)
        {
            _shopLotIconSprite.color = Color.white;
        }
        else
        {
            _shopLotIconSprite.color = Color.yellow;
        }

        while (_pool.Count < maxLevel)
            {
                ChargeUI newCharge = Instantiate(_chargePrefab, _chargesContainer);
                _pool.Add(newCharge);
            }

        for (int i = 0; i < _pool.Count; i++)
        {
            if (i < maxLevel)
            {
                _pool[i].gameObject.SetActive(true);
                _pool[i].SetState(i < currentLevel);
            }
            else
            {
                _pool[i].gameObject.SetActive(false);
            }
        }
    }
}
