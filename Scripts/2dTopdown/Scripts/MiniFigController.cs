using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    NOTHING,
    ICED,
    PLAYER,
    HUNTER
}
/**
 */
public class MiniFigController : MonoBehaviour
{
   [HideInInspector]
    public Vector3 pos;
    public float footstep;
    public GameObject dirArrow;

    public Sprite iceImg;

    // member variables
    int mMP = 0;               // Movement Point; number of foot steps player will get after every roll of dice/ if 0, can't move
    public bool mbMovable = true;    // or mbActive? or enum status? {set it as public just for testing}
    public bool isIced;


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if(mMP > 0 && mbMovable)
        {
            Move(new Vector2(horizontalInput, verticalInput));
            
            // if hit F key, invoke skill=iceshield
            // then player can't move untill others melt the ice
            // if hit F key, invoke skill + change img to ice filtered sprite
            // then set MP to 0 
            // !!!! need to check movement, it will move on next turn even if iced
            if(Input.GetKeyDown(KeyCode.F))
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = iceImg;
                mMP = 0;
                mbMovable = false;
            }
        }

        // just for testing
        if(isIced)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = iceImg;
            mbMovable = false;
        }

        //RayTest();
        //pos = transform.position;//new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    bool IsCollided(Vector2 dir)
    {
        //Ray2D ray = new Ray2D(new Vector2(transform.position.x, transform.position.y), direction);
        Vector2 origin = new Vector2(transform.position.x + dir.x/2, transform.position.y + dir.y/2);
        //Vector2 dir = transform.right;               // testing
        float distance = 1f;
        Ray2D ray = new Ray2D(origin, dir);
        Vector3 r = new Vector3(origin.x, origin.y, 0);
        Debug.DrawRay(r, new Vector3(dir.x, dir.y, 0), Color.red);

        
        RaycastHit2D hit;
        hit = Physics2D.Raycast(origin, dir, distance, LayerMask.GetMask("Obstacles"));    // what about other player? if incluing player to obstacles, then won't move/ keep getting self
        //Debug.Log();
        if (hit)
        {
            Debug.Log(hit.collider.name);
            return true;
        }

        return false;
    }

    public void Move(Vector2 dir)
    {
        Vector2 currPos, prevPos;
        currPos = transform.position;
        // get input 
        // move right
        // move left
        // move up
        // move down
        // move 1 by each input
        if(Input.GetKeyDown(KeyCode.D))
        {
            // if not colliding, then move
            if(!IsCollided(Vector2.right))
            {
                transform.position = new Vector2(currPos.x + 1, currPos.y);
                mMP--;    
            }
            // Otherwise, do nothing   
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!IsCollided(Vector2.left))
            {
                transform.position = new Vector2(currPos.x - 1, currPos.y);
                mMP--;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {   
            if(!IsCollided(Vector2.up))
            {
                transform.position = new Vector2(currPos.x, currPos.y + 1);
                mMP--;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(!IsCollided(Vector2.down))
            {
                transform.position = new Vector2(currPos.x, currPos.y - 1);
                mMP--;
            }
        }

        prevPos = currPos;
        currPos = transform.position;
        //
    }
    
    // roll dice to get nfootstep to move
    public void RollDice()
    {
        if(mbMovable)
        {
            //get random number between 1 - 6
            mMP = Random.Range(1, 6);
            if (mMP > 0)
            {
                mbMovable = true;
            }
        }
        
    }
    private void OnMouseDown()
    {
        if(isIced)
        {
            Debug.Log("Clicked!");
        }
    }

    // public functions
    // accessor
    public int GetMP()
    {
        return mMP;
    }

    
}
