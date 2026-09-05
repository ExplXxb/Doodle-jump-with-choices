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
    }

    private void Awake()
    {
        if (_textMeshProUGUI == null)
        {
            Debug.LogWarning($"{nameof(ScoreHolder)}: відсутній TextMeshProUGUI на {name}", this);
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        _scoreSystem.OnScoreChanged += ScoreSystem_OnScoreChanged;

        UpdateScore(_scoreSystem.Score);
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
        _textMeshProUGUI.text = _textBeforeScore + newScore;
    }
}
