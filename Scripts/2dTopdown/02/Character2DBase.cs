using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * *******************
 * PUBLIC FUNCTIONS
 * *******************
 * - void SetInput()
 * - void Move(vector2 dir)
 * *******************
 *PRIVATE FUNCTIONS
 * *******************
 */

public class Character2DBase : Raycaster_TopDown
{
    Animator animator;
    Vector2 input;
    bool mbIsMoving;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input.x == 0f && input.y == 0f)
        {
            mbIsMoving = false;
        }
        else
        {
            mbIsMoving = true;

        }

        // should be inside of player controller
        animator.SetBool("IsMoving", mbIsMoving);
        animator.SetFloat("DirectionX", input.x);
        animator.SetFloat("DirectionY", input.y);

        bool isMovingUpDown = false;

        // for animation
        if (input.y != 0)
        {
            isMovingUpDown = true;
        }
        else
        {
            isMovingUpDown = false;
        }
        animator.SetBool("IsMovingUpDown", isMovingUpDown);

    }
    // this should be deleted after connected with playercontroller for player
    private void FixedUpdate()
    {
        SetInput();
        Move(input);
    }
    
    public void SetInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    
    /**
     * PROBLEM
     * -
     */
    // Before moving, check collisition in 4 directions seperately
    public void Move(Vector2 dir)
    {
        // E
        if (dir.x > 0f && !mbIsCollided_E)
        {
            //Debug.Log("East");
            // Check RayEast
            DrawRaycasters(Vector2.right);
            mTransform.Translate(dir.x * Time.deltaTime * speed, 0, 0);

        }
        // W
        if (dir.x < 0f)
        {
            //Debug.Log("West");
            DrawRaycasters(Vector2.left);
            if (!mbIsCollided_W)
            {
                mTransform.Translate(dir.x * Time.deltaTime * speed, 0, 0);
            }
        }
        // S
        if (dir.y < 0f && !mbIsCollided_S)
        {
            //Debug.Log("South");
            DrawRaycasters(Vector2.down);
            //if (!mbIsCollided_S) 
            //{
            mTransform.Translate(0, dir.y * Time.deltaTime * speed, 0);
            //}
        }
        // N
        if (dir.y > 0f && !mbIsCollided_N)
        {
            //Debug.Log("North");
            DrawRaycasters(Vector2.up);
            //if(!mbIsCollided_N)
            //{
            mTransform.Translate(0, dir.y * Time.deltaTime * speed, 0);
            //}
        }

    }
}

