using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreen : MonoBehaviour
{
    private float StartTime = 5.0f;
    private float EndTime = 0.0f;
    private void Update()
    {
        StartTime -= Time.deltaTime;
        if (StartTime <= EndTime)
        {
            Debug.Log("LoadingMainMenu");
            SceneManager.LoadScene("MainMenu_Proto3_Scene");
        }
    }
}
