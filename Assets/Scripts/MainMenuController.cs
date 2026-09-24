using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
