using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    public Animator animator;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);
        animator.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        if (Input.GetButtonDown("Jump"))
        {
            player.bLanded = false;
            animator.SetBool("IsJumping", true);
            player.OnJumpInputDown();
        }
        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }
        if (player.bLanded)
        {
            animator.SetBool("IsJumping", false);
        }


    }
}
