using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Start_Menu : MonoBehaviour
{
    
    public UIDocument MainMenu_uidoc;
    public Button Play_Button;
    public Button Exit_Button;
    public void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if (MainMenu_uidoc == null)
        {
            return;
        }
        MainMenu_uidoc = GetComponent<UIDocument>();
        var MainMenu_root = MainMenu_uidoc.rootVisualElement; //Assigns temporary variable to visual elemenet of ui doc
    //Button queries
        Play_Button = MainMenu_root.Q<Button>("Star_Game_button");
        Exit_Button = MainMenu_root.Q<Button>("Exit_Game_button");
    }
    public void Update()
    {
        Play_Button.RegisterCallback<ClickEvent>(Start_Button_Click);
        Exit_Button.RegisterCallback<ClickEvent>(Exit_Button_Click);

    }
    public void Start_Button_Click(ClickEvent click)
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void Exit_Button_Click(ClickEvent click)
    {
        Debug.Log("Exting Application");
        Application.Quit();

    }
}
