using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{
    private ShopController _shopController;

    [Inject]
    public void Construct(ShopController shopController)
    {
        _shopController = shopController;
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnShopButtonClick()
    {
        _shopController.Show();
    }

    public void OnShopExitButtonClick()
    {
        _shopController.Hide();
    }
}
