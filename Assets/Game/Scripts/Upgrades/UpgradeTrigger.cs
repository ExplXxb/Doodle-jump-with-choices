using System;
using UnityEngine;
using VContainer;

public class UpgradeTrigger : MonoBehaviour
{
    public event Action OnUpgradeTriggered;

    [SerializeField] private float _heightStepMultiplier = 1.4f;
    [SerializeField] private float _heightStep = 100f;

    private float _nextTriggerHeight;

    private Player _player;

    public void Construct(Player player)
    {
        _player = player;

        _nextTriggerHeight = _heightStep;
        _player.OnMaxHeightChanged += HandleMaxHeightChanged;
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnMaxHeightChanged -= HandleMaxHeightChanged;
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