using System;
using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private float _heightStepMultiplier = 1.4f;
    [SerializeField] private float _heightStep = 100f;
    private float _nextTriggerHeight;

    public event Action OnUpgradeTriggered;

    private void Start()
    {
        _nextTriggerHeight = _heightStep;
        Player.Instance.OnMaxHeightChanged += HandleMaxHeightChanged;
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnMaxHeightChanged -= HandleMaxHeightChanged;
        }
    }

    private void HandleMaxHeightChanged(float newHeight)
    {
        if (newHeight < _nextTriggerHeight)
            return;
        
        CalculateNextTriggerHeight();
        OnUpgradeTriggered?.Invoke();
    }

    private void CalculateNextTriggerHeight()
    {
        _heightStep *= _heightStepMultiplier;
        _nextTriggerHeight += _heightStep;
    }
}