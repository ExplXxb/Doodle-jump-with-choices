using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject _shopLotPrefab;
    [SerializeField]  private GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Range(0f, 1f)]
    private float _alphaTargetValue = 0.95f;
    [SerializeField, Min(0f)]
    private float _fadeDuration = 0.5f;


    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private MetaProgressService _progress;
    private List<ShopLotPresenter> _presenters = new();

    public bool IsShowed { get; private set; }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    [Inject]
    public void Construct(ShopConfig config, MetaProgressService progress)
    {
        _progress = progress;

        _progress.OnTotalGoldChanged += UpdateAllLots;

        foreach (var upgrade in config.Upgrades)
        {
            var lotView = Instantiate(_shopLotPrefab, _gridLayoutGroup.transform).GetComponent<ShopLotView>();

            var presenter = new ShopLotPresenter(lotView, upgrade.Key, upgrade.Value, _progress);
            _presenters.Add(presenter);
        }
    }

    public void Show()
    {
        UpdateAllLots(_progress.TotalGold);

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(_alphaTargetValue, true));
    }

    public void Hide()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(0, false));
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool isVisible)
    {
        if (isVisible) _canvasGroup.blocksRaycasts = true;

        float startAlpha = _canvasGroup.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
        _canvasGroup.interactable = isVisible;
        _canvasGroup.blocksRaycasts = isVisible;
    }

    private void UpdateAllLots(int totalGold)
    {
        foreach (var presenter in _presenters)
        {
            presenter.UpdateView();
        }
    }

    private void OnDestroy()
    {
        if (_progress != null)
            _progress.OnTotalGoldChanged -= UpdateAllLots;
    }
}