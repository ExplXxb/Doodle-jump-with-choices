using TMPro;
using UnityEngine;
using VContainer;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private MetaProgressService _progress;

    [Inject]
    public void Construct(MetaProgressService progress)
    {
        _progress = progress;
        UpdateTotalGold(progress.TotalGold);
        _progress.OnTotalGoldChanged += UpdateTotalGold;
    }

    public void OnDestroy()
    {
        if (_progress != null) _progress.OnTotalGoldChanged -= UpdateTotalGold;
    }

    private void UpdateTotalGold(int newValue)
    {
        _text.text = newValue.ToString();
    }
}
