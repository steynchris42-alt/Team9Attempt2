using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Interactable_new : MonoBehaviour
{
    public Transform Player;

    private UIDocument Interact_ui;
    private Image Interact_Image;
    private Label Interact_Prompt;

    public PlayerController Player_Scr;
    public Interactable_Tracker tracker_Scr;

    private float DisToPlayer;

    public bool isClearToShow; // returns false if Notes are displaying
    public int iInteractable_Tracker;

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
        if (DisToPlayer <= 5.0f && isClearToShow )
        {
            tracker_Scr.TrackerValue_Inactive = tracker_Scr.TrackerValue_Active; //Sets the tracker to its actual value so that it triggers teh state switch in Notes_Popup
            Show_Interact_Prompt();
            Player_Scr.isAbleToInteract = true;
            Debug.Log("PlayerInRange");
        }
        else if (DisToPlayer > 5 )
        {
            tracker_Scr.TrackerValue_Active = tracker_Scr.TrackerValue_Inactive; //sets the tracker back to zero to avoid conflicts when player is near other interacables
            Hide_Interact_Prompt();
            Player_Scr.isAbleToInteract = false;
        }
    }
      public void Show_Interact_Prompt()
    {
        
            Interact_ui = Interact_ui.GetComponent<UIDocument>();
            if (Interact_ui == null)
            {
                return;
            }
            Interact_ui.rootVisualElement.style.display = DisplayStyle.Flex;
        
    }
    public void Hide_Interact_Prompt()
    {
      
            Interact_ui = Interact_ui.GetComponent<UIDocument>();
            if (Interact_ui == null)
            {
                return;
            }
            Interact_ui.rootVisualElement.style.display = DisplayStyle.None;
        
    }
}
///-----Archive---///
/// First attempt at tracking iInteractables
///      /*public void Awake()

/*
    if (gameObject.name == "Interactable")
    {
        iInteractable_Tracker = 0;
    }
    else if (gameObject.name == "Interactable (1)")
    {
        iInteractable_Tracker = 1;
    }
    else if (gameObject.name == "Interactable (2)")
    {
        iInteractable_Tracker = 2;
    }
    else if (gameObject.name == "Interactable (3)")
    {
        iInteractable_Tracker = 3;
    }
    else if (gameObject.name == "Interactable (4)")
    {
        iInteractable_Tracker = 4;
    }

*/

