using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class Player_AnimatorController : MonoBehaviour
{
    public Animator Player_Anim;
    public PlayerController Player;

    public Coroutine PlayerMove_Coro;
    private void Awake()
    {
        if (Player_Anim == null) { Player_Anim = GetComponent<Animator>(); }
    }
    void Start()
    {



    }
    void Update()
    {
        if (Player.isMoving)
        {
            Player_Anim.SetBool("isMoving_Anim", true);
            if (Player.isSprinting)
            {
                Player_Anim.SetFloat("SpeedMultiplier", 2f);
            }
            else
            {
                Player_Anim.SetFloat("SpeedMultiplier", 1);
            }
     
            // PlayerMove_Coro = StartCoroutine(PlayerMove_AnimLogic());
        }
        else
        {
            Player_Anim.SetBool("isMoving_Anim", false);
        }
    }


}