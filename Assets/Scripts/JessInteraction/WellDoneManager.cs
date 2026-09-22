using UnityEngine;
using UnityEngine.SceneManagement;

public class WellDoneManager : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Small_Town_1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
