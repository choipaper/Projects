using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    Player player;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        // The steps should be different in each level
        // this part should be fixed 
        player.Init((int)eLevels.LEVEL01);
        //player.SetSetpsByLevel((int)UIManager.eLevels.LEVEL01);
    }

    // Update is called once per frame
    void Update()
    {
        //Move player up and deduct num of steps
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResetAnimtorOnMove();
            player.Move(Vector2.up);
        }
        //Move player down and deduct num of steps
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ResetAnimtorOnMove();
            player.Move(Vector2.down);
        }
        //Move player left and deduct num of steps
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ResetAnimtorOnMove();
            player.Move(Vector2.left);
        }
        //Move player right and deduct num of steps
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ResetAnimtorOnMove();
            player.Move(Vector2.right);
        }

        if(player.bHurt)
        {
            animator.SetBool("IsHurt", true);
        }
        if(player.bIsArrived)
        {
            animator.SetBool("IsComplete", true);
        }
        if(player.bPlus)
        {
            animator.SetBool("IsPlus", true);
        }
    }

    public void ResetAnimtorOnMove()
    {
        player.bPlus = false;
        animator.SetBool("IsPlus", false);
    }

}
