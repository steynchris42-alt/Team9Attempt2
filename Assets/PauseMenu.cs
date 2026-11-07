using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
    }

    public void Home()
    {
        Time.timeScale = 1f; // Always reset time scale before changing scenes
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Unfreeze the game before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
