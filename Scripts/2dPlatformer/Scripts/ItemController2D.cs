using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class ItemController2D : MonoBehaviour
{
    public Animator animator;
    public GameObject item;
    public uint itemScore;
    public Player player;
    Rigidbody2D gravity;

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;    
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            //Debug.Log("hit detect");
            animator.SetBool("IsTaken", true);
            player.AddScore(itemScore); 
        }
        Destroy(item);
    }



}
