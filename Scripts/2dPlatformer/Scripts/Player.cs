using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // Player physics
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;
    private float movingSpeed = 6;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;           // public for debugging purpose
    private float velocityXSmoothing;
    [HideInInspector]
    public bool bLanded;
    public bool bDead;
    public GameObject dyingPoint;

    // Player Inputs
    [HideInInspector]
    public Controller2D controller;           // public for debugging purpose
    private Vector2 directionalInput;

    // Player UI
    [HideInInspector]
    public uint Scores { get; set; }
    public int Health { get; set; }
    public int NumOfHearts { get; set; }
    public GameObject gameOverUI;
    

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        Scores = 0;
        Health = 5;
        NumOfHearts = 5;
        bLanded = true;
        bDead = false;
    }

    void Update()
    {
        // dying check
        if (isDead())
        {
            // Show Dead scene and go back to where it was
            // currently pause game, show gameover panel, and just go back to first play scene 
            SceneManager.LoadScene("GameOver");
        }

        calculatedVelocity();
        Debug.Log("player velocity: " + velocity.x);
        controller.Move(velocity * Time.deltaTime, directionalInput);

        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }


        UpdateHealth();
        onLanding();

        
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if(controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            //velocity.y = wallJumpClimb.y;
        }
        /*else if (directionalInput.x == 0)
        {
            velocity.x = -wallDirX * wallJumpOff.x;
            velocity.y = wallJumpOff.y;
        }
        else
        {
            velocity.x = -wallDirX * wallLeap.x;
            velocity.y = wallLeap.y;
        }
        if (controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
				{ // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
		    }
        }
        */
    }

    public void OnJumpInputUp()
    {
        if(velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }
    /*void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }*/
    void calculatedVelocity()
    {
        float targetVelocityX = directionalInput.x * movingSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    // Add scores
    public void AddScore(uint score)
    {
        Scores += score;
    }

    // Update total health
    public void UpdateHealth()
    {
        if(isDamaged())
        {
            knockback();
            Health -= 1;
        }
    }

    bool isDamaged()
    {
        // if hit by enemy (condition: player must be hit by horizontally)
        if(controller.collisions.hitTag == "Enemy")
        {
            //Debug.Log("hit by enemy");
            return true;
        }

        return false;
    }

    // when player gets damage, knockback little bit
    void knockback()
    {
        Vector3 currPos = new Vector3(transform.position.x , transform.position.y, 0f);
        if(controller.collisions.left)
        {
            transform.Translate(1f, 0.5f, 0f);
        }
        else
        {
            transform.Translate(-1f, 0.5f, 0f);
        }
        
    }

    // configure landing check (in Update())
    private void onLanding()
    {
        if(!bLanded && velocity.y == 0)
        {
            bLanded = true;
        }
    }

    // Todo: Death motion and check

    private bool isDead()
    {
        // if player falls down
        if(transform.position.y <= dyingPoint.transform.position.y)
        {
            bDead = true;
        }
        // if player has 0 health
        if(Health == 0)
        {
            bDead = true;
        }
        return bDead;
    }
}
