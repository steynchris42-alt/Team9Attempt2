using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Notes
{
    Note0, Note1, Note2, Note3, Note4,
}
public class Notes_Popup : MonoBehaviour
{
    [Header("LINKS TO UI")]
    //---LINKS TO UI----//
    [SerializeField] private GameObject Notes_UI_GameObject;
    [SerializeField] private TextMeshProUGUI Notes_Heading_Tmp;
    [SerializeField] private TextMeshProUGUI Notes_Body_Tmp;
    [SerializeField] private Button Notes_Close_Button;

    [Header("NOTES LINES")]
    //---NOTES TEXT----//
    public string[] Note_Heading_;
    public string[] Note_Body_;

    [Header("LINKS TO SCRIPTS")]
    //---LINKS TO SCRIPTS----//
    // public Interactable_Tracker tracker;
    public LineOfSight_Scr LineOfSight;

    public int TrackerValue_Active;
    public Notes notes;
    public void Awake()
    {
        Hide_Notes();
    }
    public void Start()
    {
        Notes_Close_Button.onClick.AddListener(Hide_Notes);

 
     
    }
    public void Update()
    {
        if (LineOfSight.isInteraction_Success_Notes == true)
        {
            Show_Notes();
        }
    }

    public void Hide_Notes()
    {
        Debug.Log("Closing Note");
        LineOfSight.isInteraction_Success_Notes = false;

        Notes_UI_GameObject.SetActive(false);
     
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void Show_Notes()
    {
        Debug.Log("Showing Note");
        LineOfSight.isInteraction_Success_Notes = false;
        Notes_UI_GameObject.SetActive(true);

        Note_Assignment();
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;


    }
    
    public void Note_Assignment()
    {
        switch (notes)
        {
            case Notes.Note0:

                Notes_Heading_Tmp.text = Note_Heading_[0];
                Notes_Body_Tmp.text = Note_Body_[0];
                break;
            case Notes.Note1:
                Notes_Heading_Tmp.text = Note_Heading_[1];
                Notes_Body_Tmp.text = Note_Body_[1];
                break;
            case Notes.Note2:
                Notes_Heading_Tmp.text = Note_Heading_[2];
                Notes_Body_Tmp.text = Note_Body_[2];
                break;
            case Notes.Note3:
                Notes_Heading_Tmp.text = Note_Heading_[3];
                Notes_Body_Tmp.text = Note_Body_[3];
                break;
            case Notes.Note4:
                Notes_Heading_Tmp.text = Note_Heading_[4];
                Notes_Body_Tmp.text = Note_Body_[4];
                break;


        }
    }

    public void Note_StateSwitch(Note_Tracker tracker)
    {
        switch (tracker.Note_Tracker_Value)
        {
            case 0: notes = Notes.Note0; break;
            case 1: notes = Notes.Note1; break;
            case 2: notes = Notes.Note2; break;
            case 3: notes = Notes.Note3; break;
            case 4: notes = Notes.Note4; break;
        }
    }

    public void Headings()
    {
        Note_Heading_[0] = "There is nothing for you here"; //groccery store
        Note_Heading_[1] = "Be aware!";
        Note_Heading_[2] = "You need to leave!";
        Note_Heading_[3] = "Isiah was wrong";
        Note_Heading_[4] = "TOLD YOU SO!";
    }
    public void Body()
    {
        Note_Body_[0] = "i’m one the people that used to work in this supermarket. After the Dread showed up it was pure chaos, people fought and killed each other over what little supplies we had left. This isn't a safe place for you. -The manager";
        Note_Body_[1] = "Its like those things just apeared from out of nowhere. Folks round here been calling them the, 'Dread'. Truth is, we don't know what they are, or what they want. If this note reaches anyone, please find a safe place and be careful!";
        Note_Body_[2] = "We're leaving now. If you're reading this, don't stay in town. And whatever you do, stay away from the mansion. The truth about The Dread may be inside those walls. I don't know what that scientist has done… but I fear he is the reason they are here.\r\n 3.Something has happened to our town. Strange creatures have appeared, and nobody knows where they came from. We call them The Dread. If you're still here, leave immediately. We don't know what they want.";
        Note_Body_[3] = "Now what will you do in the day of punishment,\r\nAnd in the devastation which will come from afar?\r\nTo whom will you flee for help?\r\nAnd where will you leave your wealth? Isaiah 10:3. Father Dale used to recitet this quote alot during his sermons, like him I used to belive that the day of reckoning would bring about a new world, ruled by justice and love, but all it brought was misery and sorrow for the people of this great town. All the while that damned scientist sits in his ivory tower realishing in the carnage that remains. Isiah knew nothing... -Sarah Applestein-";
        Note_Body_[4] = "They all laughed... I tried to tell them and what happened??? They LAUGHED. but now the only person left laughing is that good for nothing Fredrick Opendingher. Who does that guy think he is anyways? Who cares if he figured out how to open a dimenional rift anyways, Even I can do that! -Gordon";
    }
 


}

//////////////////////-------------ARCHIVE------------------------------------/////////////////////////////
///

//--THE OLD NOTES POPUP SYSTEM USING UI TOOLKIT--//
/*
  private UIDocument Notes_UiDOC;
    public Label heading;
    public Label body;
    private Button ClosePage;

    public string[] Note_Heading_;
    public string[] Note_Body_;

    public Interactable_new InteractSCR;
    // public Interactable_Tracker tracker;

    public Notes notes;


    public void OnEnable()
{
    Debug.Log("LetetrUiEnabled");
    Notes_UiDOC = GetComponent<UIDocument>();
    if (Notes_UiDOC == null)
    {
        return;
    }
    var NotesDoc = Notes_UiDOC.rootVisualElement;
    if (NotesDoc != null)
    {
        heading = NotesDoc.Q<Label>("Heading");
        body = NotesDoc.Q<Label>("Body");
        ClosePage = NotesDoc.Q<Button>("ClosePage");
        ClosePage.RegisterCallback<ClickEvent>(OnClosePopupButtonCLick);
    }
}
public void OnClosePopupButtonCLick(ClickEvent evt)
{
    Debug.Log("cLICKED");
    Hide_Notes();
}
public void Hide_Notes()
{
    InteractSCR.isClearToShow = true;
    Notes_UiDOC = GetComponent<UIDocument>();
    if (Notes_UiDOC == null)
    {
        return;
    }

    Notes_UiDOC.rootVisualElement.style.display = DisplayStyle.None;
    UnityEngine.Cursor.visible = false;
    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    Time.timeScale = 1;
}
public void Show_Notes(Interactable_Tracker tracker)
{
    InteractSCR.isClearToShow = false;
    Notes_UiDOC = GetComponent<UIDocument>();
    if (Notes_UiDOC == null)
    {
        return;
    }
    if (tracker != null)
    {
        tracker.Note_StateSwitch();
    }
    Note_Assignment();
    Notes_UiDOC.rootVisualElement.style.display = DisplayStyle.Flex;
    UnityEngine.Cursor.visible = true;
    UnityEngine.Cursor.lockState = CursorLockMode.None;
    Time.timeScale = 0;
}

public void Note_Assignment()
{
    switch (notes)
    {
        case Notes.Note0:

            heading.text = Note_Heading_[0];
            body.text = Note_Body_[0];
            break;
        case Notes.Note1:
            heading.text = Note_Heading_[1];
            body.text = Note_Body_[1];
            break;
        case Notes.Note2:
            heading.text = Note_Heading_[2];
            body.text = Note_Body_[2];
            break;
        case Notes.Note3:
            heading.text = Note_Heading_[3];
            body.text = Note_Body_[3];
            break;
        case Notes.Note4:
            heading.text = Note_Heading_[4];
            body.text = Note_Body_[4];
            break;


    }
}


public void Headings()
{
    Note_Heading_[0] = "There is nothing for you here"; //groccery store
    Note_Heading_[1] = "Be aware!";
    Note_Heading_[2] = "You need to leave!";
    Note_Heading_[3] = "Isiah was wrong";
    Note_Heading_[4] = "TOLD YOU SO!";
}
public void Body()
{
    Note_Body_[0] = "i’m one the people that used to work in this supermarket. After the Dread showed up it was pure chaos, people fought and killed each other over what little supplies we had left. This isn't a safe place for you. -The manager";
    Note_Body_[1] = "Its like those things just apeared from out of nowhere. Folks round here been calling them the, 'Dread'. Truth is, we don't know what they are, or what they want. If this note reaches anyone, please find a safe place and be careful!";
    Note_Body_[2] = "We're leaving now. If you're reading this, don't stay in town. And whatever you do, stay away from the mansion. The truth about The Dread may be inside those walls. I don't know what that scientist has done… but I fear he is the reason they are here.\r\n 3.Something has happened to our town. Strange creatures have appeared, and nobody knows where they came from. We call them The Dread. If you're still here, leave immediately. We don't know what they want.";
    Note_Body_[3] = "Now what will you do in the day of punishment,\r\nAnd in the devastation which will come from afar?\r\nTo whom will you flee for help?\r\nAnd where will you leave your wealth? Isaiah 10:3. Father Dale used to recitet this quote alot during his sermons, like him I used to belive that the day of reckoning would bring about a new world, ruled by justice and love, but all it brought was misery and sorrow for the people of this great town. All the while that damned scientist sits in his ivory tower realishing in the carnage that remains. Isiah knew nothing... -Sarah Applestein-";
    Note_Body_[4] = "They all laughed... I tried to tell them and what happened??? They LAUGHED. but now the only person left laughing is that good for nothing Fredrick Opendingher. Who does that guy think he is anyways? Who cares if he figured out how to open a dimenional rift anyways, Even I can do that! -Gordon";
}

*/