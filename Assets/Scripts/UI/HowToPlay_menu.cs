using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HowToPlay_menu : MonoBehaviour
{
    public UIDocument HowToPlay_Ui;
    public Button Continue_button;
    public void OnEnable()
    {
        //UnityEngine.Cursor.lockState = CursorLockMode.None ;
        HowToPlay_Ui = GetComponent<UIDocument>();
        if (HowToPlay_Ui == null)
        {
            return;
        }
        var HowToPlay_Ui_temp = HowToPlay_Ui.rootVisualElement;
        if (HowToPlay_Ui_temp != null)
        {
            Continue_button = HowToPlay_Ui_temp.Q<Button>("Continue");
            Continue_button.RegisterCallback<ClickEvent>(OnContinueButtonCLick);
        }
    }
    public void OnContinueButtonCLick(ClickEvent evt)
    {
        SceneManager.LoadScene("Small_Town_1");
    }
}
