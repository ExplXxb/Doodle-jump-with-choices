using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapEntryPoint : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Усі потрібні сервіси ініціалізовано!");
        SceneManager.LoadScene("MainMenuScene");
    }
}
