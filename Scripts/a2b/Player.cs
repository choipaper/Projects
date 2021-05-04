using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Player properties \\
    // Life - 
    // NumOfSteps - Set it by depending on Level + difficulty
    
    // moving step
    public float movingStep = 2f;
    // player's position
    [HideInInspector]
    public Vector2 dirOld;
    private Vector2 mPosition;
    // Starting & Ending Point
    public GameObject[] points = new GameObject[2];
    // arrival check
    [HideInInspector]
    public bool bIsArrived { set; get; }
    public bool bFoundItem { set; get; }
    private int mFoundItemIndex;
    public bool bHurt { set; get; }
    public bool bPlus { set; get; }

    // player direction
    private int mDir;

    // few properties set in each level
    [HideInInspector]
    public int numOfSteps { set; get; }
    public GameObject[] items;

    // Player physics \\
    // collisionMask
    public LayerMask collisionMask;
    // rigidbody
    //private Rigidbody2D rb;
    // box Collider
    public BoxCollider2D[] boxCollider;
    //public BoxCollider2D centerCollider;
    public GameObject mapSize;


    public void Init(int steps)
    {
        //rb = GetComponent<Rigidbody2D>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //mCollider = GetComponent<BoxCollider2D>();
        // Initialize player position to SP
        mPosition.x = points[0].GetComponent<Transform>().position.x;
        mPosition.y = points[0].GetComponent<Transform>().position.y;
        transform.position = mPosition;
        numOfSteps = steps;
        bIsArrived = false;
        bHurt = false;
        bPlus = false;
        // private variable initialization
        mFoundItemIndex = -1;
        mDir = 1;
        /*Debug.Log(items[0].name);*/
    }
    
    public void Move(Vector2 dir)
    {
        /*// if collision detected
        if (boxCollider.IsTouchingLayers(collisionMask))
        {
            Debug.Log("collision");
            // Check which objects was collided
            // can't pass or walk through
            //transform.position = mPosition;
        }
        else
        {
            mPosition += dir * movingStep;
            transform.position = mPosition;
        }*/

        if(dir.x != 0)
        {
            horizontalCollision(ref dir);
        }
        if(dir.y != 0)
        {
            verticalCollision(ref dir);
        }
        if(!bIsArrived && numOfSteps > 0)
        {
            if(isOnMap())
            {
                transform.position = mPosition;
                numOfSteps--;

                //check if it arrives
                checkArrival();

                // Check if found item
                findItem();
                if (bFoundItem && mFoundItemIndex != -1)
                {
                    Debug.Log("found!");
                    items[mFoundItemIndex].SetActive(false);
                    bFoundItem = false;
                    mFoundItemIndex = -1;
                }
            }
            
        }
        // Game over
        if(numOfSteps <= 0 && !bIsArrived)
        {
            Debug.Log("Game Over");
        }
        // Level Completed
        if(bIsArrived)
        {
            Debug.Log("Level Completed!");
        }
        // anytime player moves
        //bPlus = false;
        
    }

    private void horizontalCollision(ref Vector2 dir)
    {
        // left
        if(boxCollider[0].IsTouchingLayers(collisionMask))
        {
            if(dir.x > 0)
            {
                mPosition.x += mDir * dir.x * movingStep;
            }
            else
            {
                mPosition.x -= mDir * dir.x * movingStep;
            }
        }
        // right
        if(boxCollider[1].IsTouchingLayers(collisionMask))
        {
            if (dir.x > 0)
            {
                mPosition.x -= mDir * dir.x * movingStep;
            }
            else
            {
                mPosition.x += mDir * dir.x * movingStep;
            }
        }
        else
        {
            mPosition += mDir * dir * movingStep;
        }
    }

    private void verticalCollision(ref Vector2 dir)
    {
        // up
        if (boxCollider[2].IsTouchingLayers(collisionMask))
        {
            if (dir.y > 0)
            {
                mPosition.y -= mDir * dir.y * movingStep;
            }
            else
            {
                mPosition.y += mDir * dir.y * movingStep;
            }
        }
        // down
        if (boxCollider[3].IsTouchingLayers(collisionMask))
        {
            if (dir.y > 0)
            {
                mPosition.y += mDir * dir.y * movingStep;
            }
            else
            {
                mPosition.y -= mDir * dir.y * movingStep;
            }
        }
        else
        {
            mPosition += mDir * dir * movingStep;
        }
    }


    /*
     * make this function as outta control feature
    public void WeiredWall(Vector3 dir)
    {
        dirOld = dir;
        if (boxCollider.IsTouchingLayers(collisionMask))
        {
            Debug.Log("collision detected");
            mPosition -= dirOld * movingStep;
            transform.position = mPosition;
        }
        else
        {
            mPosition += dir * movingStep;
            transform.position = mPosition;
        }
    }*/

    private void checkArrival()
    {
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        Bounds dpBounds = points[1].GetComponentInChildren<SpriteRenderer>().bounds;
        if ((dpBounds.center.x >= bounds.min.x && dpBounds.center.x <= bounds.max.x) &&
                (dpBounds.center.y >= bounds.min.y && dpBounds.center.y <= bounds.max.y))
        {
            bIsArrived = true;
        }
        if (bIsArrived)
        {
            Debug.Log("Level finished");
        }
    }

  /*  private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.tag);
        *//*if(collision.collider.CompareTag("DP"))
        {
            bIsArrived = true;
        }
        if(collision.collider.CompareTag("Items"))
        {
            Debug.Log("took Item");
        }*//*
    }*/

    // Get item's location
    public void InitItems(ref GameObject[] _items)
    {
        items = new GameObject[_items.Length];
        for(int i = 0; i < items.Length; i++)
        {
            items[i] = _items[i];
        }
    }

    private void findItem()
    {
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        //Debug.Log("player min: " + bounds.min);
        //Debug.Log("player max: " + bounds.max);
        for (int i = 0; i < items.Length; i++)
        {
            Bounds itemBound = items[i].GetComponent<SpriteRenderer>().bounds;
            //Debug.Log("item : " + itemBound.center);
            //Debug.Log("item max: " + itemBound.max);
            if ((itemBound.center.x >= bounds.min.x && itemBound.center.x <= bounds.max.x) &&
                (itemBound.center.y >= bounds.min.y && itemBound.center.y <= bounds.max.y))
            {
                Debug.Log("found!");
                bFoundItem = true;
                mFoundItemIndex = i;
                ItemEffects(i);        
            }
        }
        
    }

    private void ItemEffects(int index)
    {
        switch(index % 5)
        {
            case 0:
                numOfSteps += 2;
                bPlus = true;
                break;
            case 1:
                numOfSteps = 0;
                bHurt = true;
                break;
            case 2:
                mPosition.x = points[0].GetComponent<Transform>().position.x;
                mPosition.y = points[0].GetComponent<Transform>().position.y;
                break;
            case 3:
                points[1].transform.position = new Vector3(points[0].GetComponent<Transform>().position.x,
                    points[0].GetComponent<Transform>().position.y, -1);
                break;
            case 4:
                mDir = -1;
                break;
        }
        
    }

    private bool isOnMap()
    {
        bool flag = true;

        Bounds mapBounds = mapSize.GetComponent<MeshRenderer>().bounds;
        if(mPosition.x > mapBounds.max.x || mPosition.x < mapBounds.min.x
            || mPosition.y > mapBounds.max.y || mPosition.y < mapBounds.min.y)
        {
            flag = false;
            numOfSteps = 0;
        }

        return flag;
    }
}
