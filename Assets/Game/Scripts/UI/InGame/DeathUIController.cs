using System.Collections;
using UnityEngine;

public class DeathUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private PlayerDeathHandler _playerDeathHandler;

    [SerializeField, Range(0f, 1f)]
    private float _alphaTargetValue = 0.8f;
    [SerializeField, Min(0f)]
    private float _appearingTime = 0.5f;
    [SerializeField, Min(0f)]
    private float _uiInteractionDelay = 0.2f;

    private Coroutine _showingCoroutine;

    public bool IsShowed {  get; private set; }

    private void Awake()
    {
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        _playerDeathHandler.OnDeathUIStart += Show;
    }

    private void OnDisable()
    {
        _playerDeathHandler.OnDeathUIStart -= Show;
    }

    private void Show()
    {
        if (_showingCoroutine != null)
            return;

        _showingCoroutine = StartCoroutine(ShowingRoutine());
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator ShowingRoutine()
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

            if (!IsShowed && elapsedTime >= _appearingTime - _uiInteractionDelay)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            yield return null;
        }

        _canvasGroup.alpha = _alphaTargetValue;
        IsShowed = true;

        _showingCoroutine = null;
    }
}