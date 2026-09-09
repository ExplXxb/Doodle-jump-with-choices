using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _purchasedColor = Color.green;
    [SerializeField] private Color _unpurchasedColor = Color.gray;

    public void SetState(bool isPurchased)
    {
        _image.color = isPurchased ? _purchasedColor : _unpurchasedColor;
    }
}
