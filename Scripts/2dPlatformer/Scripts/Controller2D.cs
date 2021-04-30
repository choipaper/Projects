using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionInfo
{
    public bool above, below;
    public bool left, right;

    public bool climbingSlope;
    public bool descendingSlope;
    public bool slidingDownMaxSlope;

    public float slopeAngle, slopeAngleOld;
    public Vector2 slopeNormal;
    public Vector2 movingDistOld;
    public int faceDir;
    // hit object
    public string hitTag;
    //public bool fallingThroughPlatform;

    public void Reset()
    {
        above = below = false;
        left = right = false;
        climbingSlope = false;
        descendingSlope = false;
        slidingDownMaxSlope = false;
        slopeNormal = Vector2.zero;
        slopeAngleOld = slopeAngle;
        slopeAngle = 0;
        hitTag = "";
    }
}

public class Controller2D : RaycastController
{
    public float maxSlopeAngle = 80;

    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;
    private bool mbFacingRight = true;
    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }
    /*
     * used 
     * passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }*/

    /*
     * Move object
     * @param Vector2 moveAmount - amount of movement, 
     *        Vector2 input
     *        bool standingOnPlatform
     */
    public void Move(Vector2 movingDist, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.movingDistOld = movingDist;
        playerInput = input;

        // if going down
        if(movingDist.y < 0)
        {
            DescendSlope(ref movingDist);
        }

        // if moving left or right
        if(movingDist.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(movingDist.x);
            
           /* // Flip obj sprite
            // if moving left
            if(movingDist.x < 0 && collisions.faceDir == -1 )
            {
                if(mbFacingRight)
                {
                    flip();
                }
            }
            // if moving right
            else if (movingDist.x > 0 && collisions.faceDir == 1)
            {
                if(!mbFacingRight)
                {
                    flip();
                }
            }*/
            
        }

        // Detect collisions
        HorizontalCollisions(ref movingDist);

        // if moving up or down
        if(movingDist.y != 0)
        {
            // Detect collisions
            VerticalCollisions(ref movingDist);
        }

        // translate position of object to moved location
        transform.Translate(movingDist);

        // if standing on ground(or any platform)
        if(standingOnPlatform)
        {
            collisions.below = true;
        }
    }
    
    // start here 6/3
    void HorizontalCollisions(ref Vector2 movingDist)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(movingDist.x) + skinWidth;

        // Draw rays for physical collision detection
        for(uint i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // Draw rays
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            // if hit
            if(hit)
            {
                if (hit.collider.CompareTag("Tiles"))
                {
                    continue;
                }
                else
                {
                    collisions.hitTag = hit.collider.tag;
                }

                if (hit.distance == 0)
                {
                    continue;
                }

                // Get slope angle
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // if collides with slope
                if(i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    // if going down slope
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        movingDist = collisions.movingDistOld;
                    }

                    // To avoid air walking
                    float distanceToSlopeStart = 0;
                    
                    // if climbing new angle slope
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        movingDist.x += distanceToSlopeStart * directionX;
                    }

                    // Climb slope
                    ClimbSlope(ref movingDist, slopeAngle, hit.normal);
                    movingDist.x += distanceToSlopeStart * directionX;
                }

                // if not climing slope or isn't slope(or wall(bigger than max slope))
                if(!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    //
                    movingDist.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // To fix bouncing up and down when facing object or wall while climbing 
                    if(collisions.climbingSlope)
                    {
                        movingDist.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movingDist.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }

            }
        }
    }

    void VerticalCollisions(ref Vector2 movingDist)
    {
        float directionY = Mathf.Sign(movingDist.y);
        float rayLength = Mathf.Abs(movingDist.y) + skinWidth;

        for (uint i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + movingDist.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit)
            {
                /*if (hit.collider.CompareTag("Tiles"))
                {
                    continue;
                }
                else
                {
                    collisions.hitTag = hit.collider.tag;
                }*/
                /*
                // disable collisions for thgroughable platform
                if(hit.collider.CompareTag("Through"))
                {
                    if(directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if(collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if(playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }*/

                movingDist.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // hit while climbing slope
                if(collisions.climbingSlope)
                {
                    movingDist.x = movingDist.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(movingDist.x);
                }

                //
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        // To fix bumping at between two slopes while climbing
        if(collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(movingDist.x);
            rayLength = Mathf.Abs(movingDist.x) + skinWidth;
            // 
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * movingDist.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                // if facing new angled slope
                if(slopeAngle != collisions.slopeAngle)
                {
                    movingDist.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 movingDist, float slopeAngle, Vector2 slopeNormal)
    {
        float movingDistance = Mathf.Abs(movingDist.x);
        // In trigonometry, O = H * sin(theta)
        float climbMovingDistY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * movingDistance;

        // if climbing slope
        // because of jumping case movingDist.y > climbMovingDistY
        if (movingDist.y <= climbMovingDistY)
        {
            movingDist.y = climbMovingDistY;
            movingDist.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * movingDistance * Mathf.Sign(movingDist.x);
            collisions.below = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    void DescendSlope(ref Vector2 movingDist)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(movingDist.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(movingDist.y) + skinWidth, collisionMask);

        // 
        if(maxSlopeHitLeft || maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref movingDist);
            SlideDownMaxSlope(maxSlopeHitRight, ref movingDist);
        }

        // 
        if(!collisions.slidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(movingDist.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movingDist.x))
                    {
                        float movingDistance = Mathf.Abs(movingDist.x);
                        float descendMovingDistY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * movingDistance;
                        movingDist.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * movingDistance * Mathf.Sign(movingDist.x);
                        movingDist.y = -descendMovingDistY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                        collisions.below = true;
                        collisions.slopeNormal = hit.normal;
                    }
                }
            }
        }
    }
    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 movingDist)
    {
        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle > maxSlopeAngle)
            {
                movingDist.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(movingDist.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }

    /*void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }*/

    private void flip()
    {
        mbFacingRight = !mbFacingRight;
        transform.Rotate(0f, 180f, 0f);
        
        
    }
}
