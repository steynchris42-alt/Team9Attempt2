using UnityEngine;
using UnityEngine.UIElements;

public class Interactable_new : MonoBehaviour
{
    public Transform Player;

    private UIDocument Interact_ui;
    private Image Interact_Image;
    private Label Interact_Prompt;
    public PlayerController Player_Scr;

    private float DisToPlayer;
    private void OnEnable()
    {
        Interact_ui = GetComponent<UIDocument>();
        if (Interact_ui == null)
        {
            return;
        }
        var Interact_UIdoc = Interact_ui.rootVisualElement;
         if (Interact_UIdoc != null)
        {
            Interact_Image = Interact_UIdoc.Q<Image>("Interaction_Symbol");
            Interact_Prompt = Interact_UIdoc.Q<Label>("Interaction_prompt");
        }
    }
    void Update()
    {
        DisToPlayer = Vector3.Distance(transform.position, Player.position);
        Debug.DrawLine(transform.position, Player.position);
        if (DisToPlayer <= 5.0f)
        {
            Interact_ui.enabled = true;
            Player_Scr.isAbleToInteract = true;
            Debug.Log("PlayerInRange");
        }
        else if(DisToPlayer > 5 )
        {
            Interact_ui.enabled = false;
            Player_Scr.isAbleToInteract = false;
        }
    }
}
