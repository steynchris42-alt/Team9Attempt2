using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Victory_Menu : MonoBehaviour
{
    public UIDocument Victory_UI;
    public Button ReturnTo_Main_button;
    public Button Exit_game_Button;
    public void OnEnable()
    {
        Victory_UI = GetComponent<UIDocument>();
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        if (Victory_UI == null)
        {
            Debug.Log("Victory Ui doc is null, Returning");
            return;
        }

        var Victory_UI_temp = Victory_UI.rootVisualElement;
        if (Victory_UI_temp != null)
        {
            Debug.Log("Victory Root visual element was found, proceed");

            ReturnTo_Main_button = Victory_UI_temp.Q<Button>("ReturnTomenu_button");
            Exit_game_Button = Victory_UI_temp.Q<Button>("ExitGame_button");

            ReturnTo_Main_button.RegisterCallback<ClickEvent>(On_ReturnToMain_Click);
            Exit_game_Button.RegisterCallback<ClickEvent>(On_Exit_Click);
        }       
    }
   public void On_ReturnToMain_Click(ClickEvent evt)
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void On_Exit_Click(ClickEvent evt)
    {
        Application.Quit();
    }




}
