using TMPro;
using UnityEngine;
using VContainer;

public class ScoreHolder : MonoBehaviour
{
    [Header("Drag'n'drop")]
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    [Header("General")]
    [SerializeField] private string _textBeforeScore = "Score: ";

    private IScoreSystem _scoreSystem;

    [Inject]
    public void Construct(IScoreSystem scoreSystem)
    {
        _scoreSystem = scoreSystem;

        _scoreSystem.OnScoreChanged += ScoreSystem_OnScoreChanged;
    }

    private void Awake()
    {
        if (_textMeshProUGUI == null)
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (_textMeshProUGUI == null)
            {
                Debug.LogError($"{nameof(ScoreHolder)}: Категорично відсутній TextMeshProUGUI на {name} або в його дітях!", this);
            }
        }
    }

    private void Start()
    {
        if (_scoreSystem != null && _textMeshProUGUI != null)
        {
            UpdateScore(_scoreSystem.Score);
        }
    }

    private void OnDestroy()
    {
        if (_scoreSystem != null)
            _scoreSystem.OnScoreChanged -= ScoreSystem_OnScoreChanged;
    }

    private void ScoreSystem_OnScoreChanged(int newScore)
    {
        UpdateScore(newScore);
    }

    public void UpdateScore(int newScore)
    {
        if (_textMeshProUGUI == null) return;

        _textMeshProUGUI.text = _textBeforeScore + newScore;
    }
}
