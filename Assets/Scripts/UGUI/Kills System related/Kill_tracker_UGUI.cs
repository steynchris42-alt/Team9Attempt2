using System.Collections;
using TMPro;
using UnityEngine;


public class Kill_tracker_UGUI : MonoBehaviour
{
    public void Awake()
    {
        KillTracker_Counter.text = iKills.ToString();
    }
    // public TextMeshProUGUI KillTracker_Text;
    public TextMeshProUGUI KillTracker_Counter;
    public TextMeshProUGUI KillTracker_Text;

    public int iKills;
    public bool isKill = false;
    private Coroutine Kill_Coro;

   
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
        iKills = Mathf.Clamp(iKills + 1, 0, 100);
        KillTracker_Counter.text = iKills.ToString();
        if (Kill_Coro == null)
        {
            Kill_Coro = StartCoroutine(KillTrackerEffect());
        }
    }
    public IEnumerator KillTrackerEffect()
    {
      KillTracker_Counter.faceColor = Color.red;
      KillTracker_Text.faceColor = Color.red;
yield return new WaitForSeconds(1.0f);
        KillTracker_Counter.faceColor = Color.blue;
        KillTracker_Text.faceColor = Color.blue;
        Kill_Coro = null;

    }


}
//=============================================ARCHIVE================================////////////////////////////

/*--USing Ui Toolkit:
 * 
    /*
        public void Start()
        {
            if (KillTracker_ui != null)
            {
                return;
            }
            else
            {

            }

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
            KillTracker_Counter.text = iKills.ToString();
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
    */