using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu_Version_Chris : MonoBehaviour
{
    [Header("UI Elements")]
    //==UI Elements==//
    [SerializeField] private Button Exit_Button;
    [SerializeField] private Button StartGame_Button;

    [Header("Sound Effects")]
    //==Sound effetcs==//
    [SerializeField] private AudioSource ButtonSound;
    [SerializeField] private AudioSource TownsdaleTheme;

    public void Awake()
    {
        Exit_Button.onClick.AddListener(ExitButton_Logic);
        StartGame_Button.onClick.AddListener(StartGameButton_logic);
    }
  public void MainMenuLogic()
   {
        TownsdaleTheme.Play();
   }
    public void StartGameButton_logic()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("Townsdale_Scene");
    }
    public void ExitButton_Logic()
    {
        ButtonSound.Play();
        Application.Quit();
    }    
}
