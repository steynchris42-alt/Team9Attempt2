using UnityEngine;

public class LineOfSight_Scr : MonoBehaviour
{
    [Header("LAYER MASKS")]
    //---LAYER MASKS----//
    public LayerMask interactable;
    public LayerMask Enemy;

    [Header("GAME OBJECTS")]
    //---GAME OBJECTS----//
    public GameObject EToInteract_UI_GameObject_Notes;

    public GameObject EToInteract_UI_GameObject_PowerUps_Type1;
    public GameObject EToInteract_UI_GameObject_PowerUps_Type2;
    public GameObject EToInteract_UI_GameObject_PowerUps_Type3;

    public GameObject Reticle_UI_GameObject;

    [Header("Raycast results NOTES")]
    //---Raycast Results----//
    public bool isRayHit_Note; //causes the E to interact prompt to apear for NOTES
    public bool isShowing_InteractPrompt_Notes; //Safeguard for Callback context
    public bool isInteraction_Success_Notes; //returns true in callback context (PLAYER CONTROLLER) if interact prompt is displaying

    [Header("Raycast results POWERUPS")]
    //---Raycast Results POWERU{S----//
    public bool IsRayHit_PowerUp_Type1;
    public bool IsRayHit_PowerUp_Type2;
    public bool IsRayHit_PowerUp_Type3;
    //----Conditions set in Player Controller---//
    public bool isInteraction_Success_PowerUps_Type1;
    public bool isInteraction_Success_PowerUps_Type2;
    public bool isInteraction_Success_PowerUps_Type3;

    public RaycastHit RayHit;
    [HideInInspector]
    public PowerUp_System LineOfSight_PowerUp_Current;
 

    public void Update()
    {
        Interact_RayCast();
        Show_InteractPrompt_Notes();

        Show_InteractPrompt_PowerUps_Type1();
        Show_InteractPrompt_PowerUps_Type2();
        Show_InteractPrompt_PowerUps_Type3();

    }
    public void Interact_RayCast()
    {
        float RayDis = 10.0f;
        bool isInteractable_Hit = Physics.Raycast(transform.position, transform.forward, out RayHit, RayDis, interactable); //Casts a ray at Interactable Layer

        if (isInteractable_Hit == true)
        {
          //=====DRAW RAY HERE  Debug.DrawRay(transform.position, transform.forward * RayDis, Color.green);
            Debug.Log("HIT INTERACTABLE");

            if (RayHit.collider.GetComponent<Note_Tracker>()) //---Logic executes if the Raycast hits a NOTE--
            {
              Note_Tracker Note_Tracker = RayHit.collider.GetComponent<Note_Tracker>();
                if (Note_Tracker != null)
                {
                    Notes_Popup NotesUi = FindFirstObjectByType<Notes_Popup>();
                    NotesUi.Note_StateSwitch(Note_Tracker);
                    Debug.Log("HIT NOTE");
                    isRayHit_Note = true;
                    LineOfSight_PowerUp_Current = null;
                }
            }
            else if (RayHit.collider.GetComponent<PowerUp_Tracker>()) //---Logic executes if the Raycast hits A POWERUP--
            {
               PowerUp_Tracker Powerup_tracker = RayHit.collider.GetComponent<PowerUp_Tracker>();
                
                if (Powerup_tracker != null)
                {
                    LineOfSight_PowerUp_Current = RayHit.collider.GetComponent<PowerUp_System>();
                    switch (Powerup_tracker.PowerUp_Tracker_ID)
                    {
                        case 1: //--- POWER UP TYPE1 ID
                     IsRayHit_PowerUp_Type1 = true;
                     IsRayHit_PowerUp_Type2 = false;
                     IsRayHit_PowerUp_Type3 = false;
                            Debug.Log("HIT Type1");
                            break;
                        case 2: //--- POWER UP TYPE2 ID
                            IsRayHit_PowerUp_Type1 = false;
                            IsRayHit_PowerUp_Type2 = true;
                            IsRayHit_PowerUp_Type3 = false;
                            Debug.Log("HIT Type2");
                            break;
                        case 3: //--- POWER UP TYPE2 ID
                            IsRayHit_PowerUp_Type1 = false;
                            IsRayHit_PowerUp_Type2 = false;
                            IsRayHit_PowerUp_Type3 = true;
                            Debug.Log("HIT Type3");
                            break;
                    }

                }
            }
        }
        else
        {
          //====DRAW RAY HERE  Debug.DrawRay(transform.position, transform.forward * RayDis, Color.red);
            IsRayHit_PowerUp_Type1 = false;
            IsRayHit_PowerUp_Type2 = false;
            IsRayHit_PowerUp_Type3 = false;
            isRayHit_Note = false;
           
        }

    }
    public void Show_InteractPrompt_Notes()
    {
        if (isRayHit_Note == true)
        {
            EToInteract_UI_GameObject_Notes.SetActive(true);
            Reticle_UI_GameObject.SetActive(false);
            isShowing_InteractPrompt_Notes = true;
        }
        else
        {
            EToInteract_UI_GameObject_Notes.SetActive(false);
            Reticle_UI_GameObject.SetActive(true);
            isShowing_InteractPrompt_Notes = false;
        }
    }
    //----Below Logic shows interaction prompt for each powerup type---
    public void Show_InteractPrompt_PowerUps_Type1()
    {
        if (IsRayHit_PowerUp_Type1 == true)
        {
            EToInteract_UI_GameObject_PowerUps_Type1.SetActive(true);
            Reticle_UI_GameObject.SetActive(false);
            isInteraction_Success_PowerUps_Type1 = true;
        }
        else
        {
            EToInteract_UI_GameObject_PowerUps_Type1.SetActive(false);
            Reticle_UI_GameObject.SetActive(true);
            isInteraction_Success_PowerUps_Type1 = false;
        }
    }
    public void Show_InteractPrompt_PowerUps_Type2()
    {
        if (IsRayHit_PowerUp_Type2 == true)
        {
            EToInteract_UI_GameObject_PowerUps_Type2.SetActive(true);
            Reticle_UI_GameObject.SetActive(false);
            isInteraction_Success_PowerUps_Type2 = true;
        }
        else
        {
            EToInteract_UI_GameObject_PowerUps_Type2.SetActive(false);
            Reticle_UI_GameObject.SetActive(true);
            isInteraction_Success_PowerUps_Type2 = false;
        }
    }
    public void Show_InteractPrompt_PowerUps_Type3()
    {
        if (IsRayHit_PowerUp_Type3 == true)
        {
            EToInteract_UI_GameObject_PowerUps_Type3.SetActive(true);
            Reticle_UI_GameObject.SetActive(false);
               isInteraction_Success_PowerUps_Type3 = true;
}
        else
        {
            EToInteract_UI_GameObject_PowerUps_Type3.SetActive(false);
            Reticle_UI_GameObject.SetActive(true);
          isInteraction_Success_PowerUps_Type3 = false;
        }
    }
}
