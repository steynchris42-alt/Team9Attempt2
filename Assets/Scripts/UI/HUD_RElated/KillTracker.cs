using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class KillTracker : MonoBehaviour
{
    public int iKills;
    private UIDocument KillTracker_ui;
    public Label TrackerLabel;
    public bool isKill = false;
    private Coroutine Kill_Coro;
    public void OnEnable()
    {
        if (KillTracker_ui != null)
        {
            return;
        }
        else
        {
            KillTracker_ui = GetComponent<UIDocument>();
        }
        TrackerLabel = KillTracker_ui.rootVisualElement.Q<Label>("Kill_Label");
    }
    public void IncreaseCount()
    {
       
      if(isKill == true)
        {
            iKills++;
            Debug.Log("icnreasing kills");
        }
  
            
            //isKill = false;
          
           
         
        
    }
   
    }

    


