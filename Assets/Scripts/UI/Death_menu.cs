using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Death_menu : MonoBehaviour
{
    public UIDocument Death_ui;

    public Button Restart_button;
    public Button ReturnTo_MainMenu_button;
    public void OnEnable()
    {
     UnityEngine.Cursor.lockState = CursorLockMode.None ;
        Death_ui = GetComponent<UIDocument>();
        if (Death_ui == null)
        {
            return;
        }
        
       var Death_Ui_temp = Death_ui.rootVisualElement;
        if (Death_Ui_temp != null)
        {
            Restart_button = Death_Ui_temp.Q<Button>("Retry");
            ReturnTo_MainMenu_button = Death_Ui_temp.Q<Button>("Return_To_MainMenu");

            Restart_button.RegisterCallback<ClickEvent>(OnRestartButtonCLick);
            ReturnTo_MainMenu_button.RegisterCallback<ClickEvent>(On_ReturnToMain_Button_Click);
        }
    }

    public void OnRestartButtonCLick(ClickEvent evt)
    {
        SceneManager.LoadScene("Small_Town_1");
    }
    public void On_ReturnToMain_Button_Click(ClickEvent evt)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
