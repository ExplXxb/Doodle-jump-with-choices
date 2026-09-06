using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void OnPlayAgainButtonClick()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnExitButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
