using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Victory_Version_Chris1 : MonoBehaviour
{
    [Header("UI Elements")]
    //==UI Elements==//
    [SerializeField] private Button Exit_ToMainMenu_Button;
    [SerializeField] private Button PlayAgain_Button;

    [Header("Sound Effects")]
    //==Sound effetcs==//
    [SerializeField] private AudioSource ButtonSound;
    [SerializeField] private AudioSource TownsdaleTheme;

    public void Awake()
    {
        Exit_ToMainMenu_Button.onClick.AddListener(Exit_ToMainMenu_Button_Logic);
        PlayAgain_Button.onClick.AddListener(PlayAgain_Button_logic);
    }
  public void MainMenuLogic()
   {
        TownsdaleTheme.Play();
   }
    public void PlayAgain_Button_logic()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("Townsdale_Scene");
    }
    public void Exit_ToMainMenu_Button_Logic()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("MainMenu_Proto3_Scene");
    }    
}
