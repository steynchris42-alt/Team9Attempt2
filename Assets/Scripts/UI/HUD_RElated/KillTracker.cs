using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;

public class KillTracker : MonoBehaviour
{
    public int iKills;
    private UIDocument KillTracker_ui;
    public Label TrackerLabel;
    public bool isKill = false;
    private Coroutine Kill_Coro;

    public PlayerController player_scr;

    public void Start()
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
    public void OnEnable()
    {
        Enemy_parent_Class.Kill_Event += IncreaseCount;
    }
    public void OnDisable()
    {
        Enemy_parent_Class.Kill_Event -= IncreaseCount;
    }

    public void IncreaseCount()
    {
        iKills++;
        TrackerLabel.text = iKills.ToString();
        switch (iKills)
        {
            case 10: SprintBoost(); break;
        }
    }
    public void PowerUps()
    {
        switch (iKills)
        {
            case 10: SprintBoost(); break;
        }
    }

    public void SprintBoost()
    {
        player_scr.SprintSpeed = 30.0f;
        Debug.Log("SPRINT BOOST ACHIEVED");
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
///--------------ARCHIVE--------------------------/////////////
///
//--OLD WIN CONDITION--
/* if (iKills == 50)
 {
     SceneManager.LoadScene("Victory");
 }
 Debug.Log("icnreasing kills"); */