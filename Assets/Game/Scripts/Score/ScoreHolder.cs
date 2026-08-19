using TMPro;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    [Header("Drag'n'drop")]
    [SerializeField] private ScoreSystem _scoreSystem;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    [Header("General")]
    [SerializeField] private string _textBeforeScore = "Score: ";
     
    private void Awake()
    {
        if (_scoreSystem == null)
        {
            Debug.LogError($"{nameof(ScoreSystem)}: відсутній ScoreSystem на {name}", this);
        }

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
