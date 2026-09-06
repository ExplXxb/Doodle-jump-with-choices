using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RarityCardPrefab
{
    public EffectRarity CardRarity;
    public GameObject CardPrefab;
}

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private List<RarityCardPrefab> _cardPrefabs;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Range(0f, 1f)]
    private float _alphaTargetValue = 0.8f;
    [SerializeField, Min(0f)]
    private float _appearingTime = 0.5f;
    [SerializeField, Min(0f)]
    private float _uiInteractionDelay = 0.2f;

    private Dictionary<EffectRarity, Stack<CardUI>> _cardsPool = new();
    private Dictionary<EffectRarity, GameObject> _prefabByRarity = new();
    private List<CardUI> _activeCards = new();
    private Coroutine _showUIRoutine;

    private void Awake()
    {
        foreach (var cardPrefab in _cardPrefabs)
        {
            _prefabByRarity.Add(cardPrefab.CardRarity, cardPrefab.CardPrefab);
        }
    }

    public void ShowCards(List<UpgradeCard> cards, Action<UpgradeCard> handleCardSelected)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardUI cardUI = GetCardFromPoolOrInstantiate(cards[i].PositiveEffect.Rarity);
            cardUI.EnterCardValues(
                cards[i], 
                card => HandleCardPicked(card, handleCardSelected),
                cards[i].PositiveEffect.DisplayName, 
                cards[i].NegativeEffect.DisplayName, 
                cards[i].PositiveEffect.DisplayText, 
                cards[i].NegativeEffect.DisplayText);
            _activeCards.Add(cardUI);
            cardUI.gameObject.SetActive(true);
        }
    }

    private CardUI GetCardFromPoolOrInstantiate(EffectRarity rarity)
    {
        if (!_cardsPool.TryGetValue(rarity, out var stack))
        {
            stack = new Stack<CardUI>();
            _cardsPool[rarity] = stack;
        }

        if (stack.Count > 0)
        {
            return stack.Pop();
        }

        var prefab = _prefabByRarity[rarity];
        return Instantiate(prefab, _gridLayoutGroup.transform).GetComponent<CardUI>();
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
            if (!_cardsPool.TryGetValue(card.Rarity, out var stack))
            {
                stack = new Stack<CardUI>();
                _cardsPool[card.Rarity] = stack;
            }
            stack.Push(card);
        }
        _activeCards.Clear();
    }

    public void ShowUI()
    {
        if (_showUIRoutine != null)
            StopCoroutine(_showUIRoutine);
        _showUIRoutine = StartCoroutine(ShowUIRoutine());
    }

    public void HideUI()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator ShowUIRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _appearingTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = elapsedTime / _appearingTime;

            _canvasGroup.alpha = Mathf.Lerp(
                0f,
                _alphaTargetValue,
                progress
            );

            if (elapsedTime >= _appearingTime - _uiInteractionDelay)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            yield return null;
        }

        _canvasGroup.alpha = _alphaTargetValue;
    }
}
