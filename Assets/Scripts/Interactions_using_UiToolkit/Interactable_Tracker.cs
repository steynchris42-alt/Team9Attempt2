using UnityEngine;
using static Notes_Popup;

public class Interactable_Tracker : MonoBehaviour
{
    public int TrackerValue_Inactive = -1;
    public int TrackerValue_Active;

    public Notes_Popup Note_Scr;
    //easy way to assign numerical values to gameobejcts for identification purposes


 
    public void Note_StateSwitch()
    {
        switch (TrackerValue_Active)
        {
            case 0: Note_Scr.notes = Notes.Note0; break;
            case 1: Note_Scr.notes = Notes.Note1; break;
            case 2: Note_Scr.notes = Notes.Note2; break;
            case 3: Note_Scr.notes = Notes.Note3; break;
            case 4: Note_Scr.notes = Notes.Note4; break;
        }
    }

}
  


