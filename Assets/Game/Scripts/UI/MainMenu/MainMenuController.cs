using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
