using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public event Action OnCardUISelected;

    [SerializeField] private TextMeshProUGUI _cardNameText;
    [SerializeField] private TextMeshProUGUI _upgradeText;
    [SerializeField] private TextMeshProUGUI _debuffText;

    private UpgradeCard _upgradeCard;
    private Action<UpgradeCard> _handleCardSelected;

    public EffectRarity Rarity => _upgradeCard.PositiveEffect.Rarity;

    private void Awake()
    {
        if (_cardNameText == null)
        {
            Debug.LogError($"{nameof(CardUI)}: в Інспекторі відсутній {nameof(TextMeshProUGUI)} на {name}", this);
        }
        if (_upgradeText == null)
        {
            Debug.LogError($"{nameof(CardUI)}: в Інспекторі відсутній {nameof(TextMeshProUGUI)} на {name}", this);
        }
        if (_debuffText == null)
        {
            Debug.LogError($"{nameof(CardUI)}: в Інспекторі відсутній {nameof(TextMeshProUGUI)} на {name}", this);
        }
    }

    public void EnterCardValues(UpgradeCard card, Action<UpgradeCard> HandleCardSelected, string cardPositiveNamePart, string cardNegativeNamePart, string upgradeText, string debuffText)
    {
        _upgradeCard = card;
        _handleCardSelected = HandleCardSelected;

        _cardNameText.text = cardNegativeNamePart + " " + cardPositiveNamePart;
        _upgradeText.text = upgradeText;
        _debuffText.text = debuffText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _handleCardSelected.Invoke(_upgradeCard);
        OnCardUISelected?.Invoke();
    }
}
