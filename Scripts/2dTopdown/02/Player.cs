using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum Status of Player
public enum EStatus
{
    Active,
    Deactive,
    Spactator,
    Sulre,
    Iced,
    Player
}

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    // public
    // private
    // memeber variables
    //--------------------------------------------------------------------------------
    //from MiniFigController.cs

    [HideInInspector]
    public Vector3 pos;
    public LayerMask collisionLayer;
    public int numOfRays = 5;
    // member variables
    public int mMP = 0;               // Movement Point; number of foot steps player will get after every roll of dice/ if 0, can't move
    public bool mbMovable = true;    // or mbActive? or enum status? {set it as public just for testing}
    public bool isIced;
    public const float skinWidth = .015f;
    public const float distBtwRays = .15f;

    // PRIVATE
    BoxCollider2D boxCollider;
    EStatus mStatus = EStatus.Deactive;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        mStatus = EStatus.Sulre;    //change status
        //Debug.Log(Mathf.Sign(0));

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        /*if (mMP > 0 && mbMovable)
        {
            Move(new Vector2(horizontalInput, verticalInput));

            // if hit F key, invoke skill=iceshield
            // then player can't move untill others melt the ice
            // if hit F key, invoke skill + change img to ice filtered sprite
            // then set MP to 0 
            // !!!! need to check movement, it will move on next turn even if iced
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = iceImg;
                mMP = 0;
                mbMovable = false;
            }
        }

        // just for testing
        if (isIced)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = iceImg;
            mbMovable = false;
        }*/

        //RayTest();
        //pos = transform.position;//new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private int calculateRayCount(float length)
    {
        int rayCount = Mathf.RoundToInt(length / (distBtwRays));

        return rayCount;
    }

    // !!!! Maybe, try to sperate between checking collision and drawing ray?
    private bool IsCollided(ref Vector2 dir)
    {
        // var
        float indent = 0.03f;        // 0.1f indentation for ray (starting from 0.1f inside of the collider)
        float rayLength = 0.03f;            // length of ray
        int nRay = numOfRays;        // temporary use
        Vector2 origin = new Vector2(transform.position.x + Mathf.Sign(dir.x) * (boxCollider.size.x / 2 - indent), transform.position.y + Mathf.Sign(dir.y) * (boxCollider.size.y / 2 - indent));


        // get box collider2d
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);                // shrink boxcollider to fire rays so that player can detect collisions from surface of the player

        // calculate number of ray for player
        int horizontalCount = calculateRayCount(boxCollider.size.x) - 1;
        int verticalCount = calculateRayCount(boxCollider.size.y) - 1;
        //Debug.Log(horizontalCount + ", " + verticalCount);
        // Then draw fire rays seperately by vertically and horizontally
        // draw horizontal ray

        // draw vertical ray
        /**
         * I need to create 5 rays toward the dir, and the ray starts 0.01f beneath of the surface of the boxcollider
         * 1. create 5 rays
         * 
         */
        Ray2D[] rays = new Ray2D[5];
        float distX = boxCollider.size.x / rays.Length;
        float distY = boxCollider.size.y / rays.Length;
        float dirSignX = dir.x == 0 ? 0 : Mathf.Sign(dir.x);
        float dirSignY = dir.y == 0 ? 0 : Mathf.Sign(dir.y);
        //Debug.Log(dirSignX + " , " + dirSignY);
        for (int i = 0; i < 5; i++)
        {
            if(dir.x != 0)
            {
                rays[i] = new Ray2D(new Vector2(origin.x, origin.y - i * distY), dir);
            }
            if(dir.y != 0)
            {
                rays[i] = new Ray2D(new Vector2(origin.x - i * distX, origin.y), dir);
            }
            
            Debug.DrawRay(rays[i].origin, rays[i].direction, Color.red);

            RaycastHit2D hit;
            hit = Physics2D.Raycast(rays[i].origin, rays[i].direction, rayLength, collisionLayer);

        

            if (hit)
            {
                if (hit.collider.CompareTag("SeeThru"))
                {
                    
                    //Debug.Log(hit.collider.name);
                }
                //Debug.Log("IsCollide(vec2):" + hit.collider.name);
                return true;
            }
            

            
        }

        /* for (int i = 0; i < numOfRays; i++)
         {
             Ray2D ray = new Ray2D(origin, dir);
             Vector3 or = new Vector3(origin.x, origin.y, 0);
             if (dir.x != 0)
             {
                 or.y -= ((boxCollider.size.y - indent * 2) / nRay);
                 Debug.DrawRay(or, new Vector3(dir.x, dir.y, 0), Color.red);
             }
             if(dir.y != 0)
             {
                 or.x -= ((boxCollider.size.x - indent * 2) / nRay);
                 Debug.DrawRay(or, new Vector3(dir.x, dir.y, 0), Color.red);
             }

         }*/
        //Debug.DrawRay(or, new Vector3(dir.x, dir.y, 0), Color.red);
        /*RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction, rayLength, collisionLayer);

        if (hit)
        {
            Debug.Log(hit.collider.name);
        }
*/
        return false;
    }

    // Check if colliding with something by firing ray to dir
  /*  bool IsCollided(ref Vector2 dir)
    {
        //-- var --
        // Here, castRay returns 5 rays
        Ray2D ray = castRay(ref dir);       // before we check the collision we need the ray to check collisions   
        


        
    }*/

    // basic movement
    // move toward the dir 
    // go to direction by one
    public void Move(ref Vector2 dir, float velocity)
    {

        //before move we need to check collision
        if (!IsCollided(ref dir))
        {
            // then we can change position
            transform.Translate(new Vector3(dir.x, dir.y, 0) * velocity);
        }
        
    }

    // this is game function - it shouldn't be here
    // roll dice to get nfootstep to move
    public void RollDice()
    {
        if (mbMovable)
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
        if (isIced)
        {
            Debug.Log("Clicked!");
        }
    }

    /**
     * Skills
     * 
     */
    // just dash for now : move toward the dir by 1.3
    public void Dash(Vector2 dir)
    {
        float dashDist = 1.3f;
        transform.position += new Vector3(dir.x * dashDist, dir.y * dashDist, 0);
    }

    // break window to enter house
    public void BreakWindow(GameObject window)
    {
        // send signal to game manager -> house obj changes entrance = true(allow players to access into the house)
    }

 
    // public functions
    // Setter
    public void SetStatus(EStatus status)
    {
        mStatus = status;
    }

    // accessor
    public int GetMP()
    {
        return mMP;
    }
    public bool GetMovable()
    {
        return mbMovable;
    }

    public EStatus GetStatus()
    {
        return mStatus;
    }

}
