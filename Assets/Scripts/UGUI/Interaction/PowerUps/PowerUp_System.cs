using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp_System : MonoBehaviour
{
    [Header("LINKS TO SCRIPTS")]
    //---LINKS TO SCRIPTS----//
   public Kill_tracker_UGUI KillTracker;
   public LineOfSight_Scr LineOfSight;
   public PlayerController PlayerController;

    [Header("POWER UP AVAILABILITY")]
    //---POWERUP LOCK SETTINSG----//
    public bool isPowUp_Available_Type1;
    public bool isPowUp_Available_Type2;
    public bool isPowUp_Available_Type3;
    [Header("POWEr Up Int COntroll")]
    //---POWERUP TYPE1----//
    public int iPowerUp_Type1;
    public int iPowerUp_Type1_Min;
    public int iPowerUp_Type1_Max;
    //---POWERUP TYPE2----//
  //  public int iPowerUp_Type2;
    public int iPowerUp_Type2_Min;
    public int iPowerUp_Type2_Max;
    //---POWERUP TYPE3----//
    //public int iPowerUp_Type3;
    public int iPowerUp_Type3_Min;
    public int iPowerUp_Type3_Max;

    [Header("UGUI")]
    //---POWERUP LOCK SETTINSG----//
    //public TextMeshProUGUI PowerUp_UI_Type1;
   // public TextMeshProUGUI PowerUp_UI_Type2;
    //public TextMeshProUGUI PowerUp_UI_Type3;

    //---Coroutines----//
    public Coroutine Purchase_Coro;

    public void PowerUp_Purchase_() //Called in Interaction call back context. Script: 'PlayerCOntroller'
    {   
      
        switch (KillTracker.iKills)
        {
            case >= 30: //makes TYPE2 available
                isPowUp_Available_Type3 = true;
                isPowUp_Available_Type2 = true;
                isPowUp_Available_Type1 = true;
                break;
            case >= 20: //makes TYPE2 available
                isPowUp_Available_Type3 = false;
                isPowUp_Available_Type2 = true;
                isPowUp_Available_Type1 = true;
                break;
            case >= 10: //makes TYPE1 available
                isPowUp_Available_Type3 = false;  
                isPowUp_Available_Type2 = false;
                isPowUp_Available_Type1 = true;
                break;
        }
        if (LineOfSight.IsRayHit_PowerUp_Type1)
        {
            isPowUp_Available_Type1_Logic(isPowUp_Available_Type1);
        }
        else if (LineOfSight.IsRayHit_PowerUp_Type2)
        {
            isPowUp_Available_Type2_Logic(isPowUp_Available_Type2);
        }
        else if (LineOfSight.IsRayHit_PowerUp_Type3)
        {
            isPowUp_Available_Type3_Logic(isPowUp_Available_Type3);
        }
    }
    #region POWERUP TYPE1 LOGIC
    public void isPowUp_Available_Type1_Logic(bool isAvailable ) 
    {
        if (isAvailable == true && Purchase_Coro == null)
        {
            Purchase_Coro = StartCoroutine(Powerup_Purchase_Coro_Type1());
            Debug.Log("Powerup type1 Purchase SUCCEED!");
        }
        else if(isAvailable == false) 
        {
            Debug.Log("Powerup type1 Purchase FAILED YOU SUCK!");
        }
    }
    public IEnumerator Powerup_Purchase_Coro_Type1()
    {
        PlayerController.iPowerUp_Type1 += 1;
    PlayerController.PowerUp_UI_Type1.text = PlayerController.iPowerUp_Type1.ToString();
    KillTracker.iKills = Mathf.Clamp(KillTracker.iKills - 10, 0, 100);
   KillTracker.KillTracker_Counter.text = KillTracker.iKills.ToString();

        if (KillTracker.iKills < 10)
        {
            isPowUp_Available_Type1 = false;
        }
            yield return null;
   Purchase_Coro = null;

    }
    #endregion
    #region POWERUP TYPE2 LOGIC
    public void isPowUp_Available_Type2_Logic(bool isAvailable)
    {
        if (isAvailable == true && LineOfSight.IsRayHit_PowerUp_Type2 && Purchase_Coro == null)
        {

            Purchase_Coro = StartCoroutine(Powerup_Purchase_Coro_Type2());
            Debug.Log("Powerup type2 Purchase SUCCEED!");
        }
        else if (isAvailable == false)
        {
            Debug.Log("Powerup type2 Purchase FAILED YOU SUCK!");
        }
    }
    public IEnumerator Powerup_Purchase_Coro_Type2()
    {
        PlayerController.iPowerUp_Type2 += 1;
        PlayerController.PowerUp_UI_Type2.text = PlayerController.iPowerUp_Type2.ToString();
        KillTracker.iKills = Mathf.Clamp(KillTracker.iKills - 20, 0, 100);
        KillTracker.KillTracker_Counter.text = KillTracker.iKills.ToString();

        if (KillTracker.iKills < 20) //MAKES POWER UP UNAVAILABLE
        {
            isPowUp_Available_Type2 = false;
        }
        yield return null;
        Purchase_Coro = null;

    }
    #endregion
    #region POWERUP TYPE3 LOGIC
    public void isPowUp_Available_Type3_Logic(bool isAvailable)
    {
        if (isAvailable == true && LineOfSight.IsRayHit_PowerUp_Type3 && Purchase_Coro == null)
        {
            Purchase_Coro = StartCoroutine(Powerup_Purchase_Coro_Type3());
            Debug.Log("Powerup type3 Purchase SUCCEED!");
        }
        else if (isAvailable == false)
        {
            Debug.Log("Powerup type3 Purchase FAILED YOU SUCK!");
        }
    }
    public IEnumerator Powerup_Purchase_Coro_Type3()
    {
        PlayerController.iPowerUp_Type3 += 1;
        PlayerController.PowerUp_UI_Type3.text = PlayerController.iPowerUp_Type3.ToString();
        KillTracker.iKills = Mathf.Clamp(KillTracker.iKills - 30, 0, 100);
        KillTracker.KillTracker_Counter.text = KillTracker.iKills.ToString();
        if (KillTracker.iKills < 20) //MAKES POWER UP UNAVAILABLE
        {
            isPowUp_Available_Type3 = false;
        }
        yield return null;
        Purchase_Coro = null;
    }
    #endregion

}

//========================ARCHIVE=================================////
/*
 * public void Purchasing()
    {
        Debug.Log("Powerup type1 Purchase SUCCEED!");
        iPowerUp_Type1 += 1;
        //  KillTracker.iKills -= 10;
        KillTracker.KillTracker_Counter.text = KillTracker.iKills.ToString();
        KillTracker.iKills = Mathf.Clamp(KillTracker.iKills - 10, 0, 100);
    }
*/