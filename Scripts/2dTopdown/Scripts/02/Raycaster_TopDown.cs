using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * sepearte to movement checker and just raycaster for shooting gun
 * if hit bounce back?
 * 
 * 
 * *******************
 * PUBLIC FUNCTIONS
 * *******************
 * - void DrawRay(Vector2 origin, Vector2 direction):single ray
 * - void ResetPos()-
 * - void SetInput()-
 * - void CollisionCheckReset()
 * - void CalculateRays()
 * *******************
 * PRIVATE FUNCTIONS
 * *******************
 * - void freezeDirection(Vector2 dir)-
 * - void move(Vector2 dir)-
 * - Vector2 getMousePos()-
 * - void fireObject()->to fireGun.cs
 * - void drawRayCaster(Vector2 direction):for player
 * 
 * Todo: finish ray casting & propper collision detection, 
 * fire object
 * patrol around by following selected points = AI
 * follow and catch + attack = AI
 * Dash
 */
[RequireComponent(typeof(BoxCollider2D))]
public class Raycaster_TopDown : MonoBehaviour
{
    public float greenRayLength = 2.5f;
    public int numOfRays = 6;
    public float rayLength = 0.24f;
    public float skinWidth = 0.02f;
    public Vector2 distanceBtwRays;
    public float speed = 2.3f;
    public LayerMask collisionLayer;
    public int footstep;
    public GameObject missile;

    public Transform mTransform;
    public BoxCollider2D BoxCollider;

    
    Vector2 freezedDir;
    public bool mbIsCollided_E, mbIsCollided_W, mbIsCollided_S, mbIsCollided_N; // public for test
    float horizontalInput;
    float inputy;

    Vector2 clickedPos;


    Vector2 bulletDirection;
    // Debug purpose

    public virtual void Awake()
    {
        mTransform = GetComponent<Transform>();
        BoxCollider = GetComponent<BoxCollider2D>();

        CollisionCheckReset();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Test reset
    public void ResetPos()
    {
        mTransform.position = new Vector3(0, 0, -0.75f);
    }

    // Update is called once per frame
    void Update()
    {

        
        // MOVE
        //move(new Vector2(horizontalInput, verticalInput));

        // Green Ray from object towards mouse position origin=foot
        Vector2 mousePos = getMousePos();
        Vector2 pos = new Vector2(mTransform.position.x, mTransform.position.y);
        Ray2D ray = DrawRay(pos, mousePos - pos);
        // SHOOTING!
        if (Input.GetKey(KeyCode.F))
        {
            GameObject bullet = fireObject(ray);
            //Vector3 rayEdge = new Vector3(ray.origin.x + rayLength, ray.origin.y + rayLength);
            //while(bullet.transform.position != rayEdge)
            //{
            if(bullet != null)
            {
                bulletDirection = ray.direction;
                //bull.SetDirection(ray.direction);
                Debug.Log("Fire!");

                //bullet.transform.Translate(ray.direction * Time.deltaTime *speed);
            }
            //Destroy(bullet);
            //}
        }

        //Test
        if (Input.GetKey(KeyCode.R))
        {
            ResetPos();
        }
        DrawRaycasters(Vector2.right);
        //followLine();
    }

    /**
     * PUBLIC FUNCTIONS
     */

    // Draw a sing ray
    // for fire a projectile
    public Ray2D DrawRay(Vector2 origin, Vector2 direction)
    {
        Ray2D ray = new Ray2D(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * greenRayLength, Color.green);
        return ray;
    }

    public void CollisionCheckReset()
    {
        mbIsCollided_E = false;
        mbIsCollided_W = false;
        mbIsCollided_S = false;
        mbIsCollided_N = false;
    }

    public Vector2 GetBulletDirection()
    {
        return bulletDirection;
    }

    /**
     * PRIVATE FUNCTIONS
     */
    // create a obj following green rays
    // can be seperated with following trail + moving 
    GameObject fireObject(Ray2D ray)
    {
        GameObject bullet;
        if (Input.GetMouseButtonDown(0))
        {

            bullet = Instantiate(missile, ray.origin, Quaternion.identity);

            /*Vector3 rayEdge = new Vector3(ray.origin.x + rayLength, ray.origin.y + rayLength);
            //while(bullet.transform.position != rayEdge)
            //{
                bullet.transform.Translate(ray.direction * Time.deltaTime * speed);
            //}
            */
            return bullet;
        }

        return null;

    }

    Vector2 getMousePos()
    {
        Vector2 mousePos;
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //convert to screen position
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        return screenPos;
    }

    public void CalculateRays()
    {
        Bounds bounds = BoxCollider.bounds;
        bounds.Expand(skinWidth * -1);
        float width = bounds.size.x;
        float height = bounds.size.y;
    }
    /**
     * Problem
     * - when pressed two direction at the same time ex)NW -> walk through
     * a object which is slightly bump out than the current object
     */
    // draw rays for 4 directions
    public void DrawRaycasters(Vector2 direction)
    {
        // 4Debug
        //string debug = "";
        Bounds bounds = BoxCollider.bounds;
        bounds.Expand(skinWidth * -1);

        // calculate size of box collider
        bounds.Expand(skinWidth * -1);
        Vector2 upperLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        float width, height;
        
        width = bounds.extents.x * 2;
        height = bounds.extents.y * 2;
        Vector2 distBtwRays = new Vector2(width / (numOfRays - 1), height/(numOfRays - 1));
       
        // Always East,West,South,West 
        Ray2D[] raysE = new Ray2D[numOfRays];
        Ray2D[] raysW = new Ray2D[numOfRays];
        Ray2D[] raysS = new Ray2D[numOfRays];
        Ray2D[] raysN = new Ray2D[numOfRays];
        RaycastHit2D hitE, hitW, hitS, hitN;

        for (int i = 0; i < numOfRays; i++)
        {
            //East
            raysE[i] = new Ray2D(new Vector2(bottomRight.x, bottomRight.y + i * distBtwRays.y), Vector2.right);
            //West
            raysW[i] = new Ray2D(new Vector2(upperLeft.x, upperLeft.y - i * distBtwRays.y), Vector2.left);
            //South
            raysS[i] = new Ray2D(new Vector2(bottomRight.x - i * distBtwRays.x, bottomRight.y), Vector2.down);
            //North
            raysN[i] = new Ray2D(new Vector2(upperLeft.x + i * distBtwRays.x, upperLeft.y), Vector2.up);

            Debug.DrawRay(raysE[i].origin, raysE[i].direction * rayLength, Color.red);
            Debug.DrawRay(raysW[i].origin, raysW[i].direction * rayLength, Color.red);
            Debug.DrawRay(raysS[i].origin, raysS[i].direction * rayLength, Color.blue);
            Debug.DrawRay(raysN[i].origin, raysN[i].direction * rayLength, Color.blue);
        }

        for (int i = 0; i < numOfRays; i++)
        {
            hitE = Physics2D.Raycast(raysE[i].origin, raysE[i].direction * rayLength, rayLength, collisionLayer);
            hitW = Physics2D.Raycast(raysW[i].origin, raysW[i].direction * rayLength, rayLength, collisionLayer);
            hitS = Physics2D.Raycast(raysS[i].origin, raysS[i].direction * rayLength, rayLength, collisionLayer);
            hitN = Physics2D.Raycast(raysN[i].origin, raysN[i].direction * rayLength, rayLength, collisionLayer);
            // whatever it is, if it hit, then turn on the direction of ray -> then cannot move to that direction
            if (hitE)
            {
                mbIsCollided_E = true;
                // freeze right key
                //freezeDirection(Vector2.right);
                //transform.Translate(, 0, 0);
                //debug = "HitE";
                // block
                //transform.Translate(direction);
                break;
            }
            if (hitW)
            {
                mbIsCollided_W = true;
                //debug += ", HitW";
                //freezeDirection(Vector2.left);
                break;
            }
            if (hitS)
            {
                mbIsCollided_S = true;
                //freezeDirection(Vector2.down);
                //debug += ", HitS";
                break;
            }
            if (hitN)
            {
                mbIsCollided_N = true;
                //freezeDirection(Vector2.up);
                //debug += ", HitN";
                break;
            }
            else
            {
                //debug = "No COLLISION";
                mbIsCollided_E = false;
                mbIsCollided_W = false;
                mbIsCollided_S = false;
                mbIsCollided_N = false;
            }
        }
    }
}
