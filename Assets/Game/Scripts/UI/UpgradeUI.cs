using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    private Stack<CardUI> _cardsPool = new Stack<CardUI>();
    private Stack<CardUI> _activeCards = new Stack<CardUI>();

    public void ShowCards(List<UpgradeCard> cards, Action<UpgradeCard> handleCardSelected)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardUI cardUI = GetCardFromPoolOrInstantiate();
            cardUI.EnterCardValues(
                cards[i], 
                card => HandleCardPicked(card, handleCardSelected),
                cards[i].PositiveEffect.DisplayName, 
                cards[i].NegativeEffect.DisplayName, 
                cards[i].PositiveEffect.DisplayText, 
                cards[i].NegativeEffect.DisplayText);
            _activeCards.Push(cardUI);
            cardUI.gameObject.SetActive(true);
        }
    }

    private CardUI GetCardFromPoolOrInstantiate()
    {
        return _cardsPool.Count > 0
            ? _cardsPool.Pop() 
            : Instantiate(_cardPrefab, _gridLayoutGroup.transform).GetComponent<CardUI>();
    }

    private void HandleCardPicked(UpgradeCard card, Action<UpgradeCard> handleCardSelected)
    {
        HideCardsInPool();
        handleCardSelected.Invoke(card);
    }

    private void HideCardsInPool()
    {
        foreach (var card in _activeCards)
        {
            card.gameObject.SetActive(false);
            _cardsPool.Push(card);
        }
        _activeCards.Clear();
    }

    public void ShowUI()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void HideUI()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
