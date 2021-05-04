using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Player controller is for turn base games
 */
[RequireComponent(typeof(Player))]
public class PlayerController_Turn : MonoBehaviour
{
    //[__________Public__________] 
    public GameObject startingPoint;
    public GameObject handPoint;
    public float moveSpeed = 0.3f;
    //lights
    //public GameObject auralight;
    //public GameObject flashlight;
    //Game manager
    //public GameManager gm;


    public float testDeg;
    public float rotationSpeed;
    //[__________Private__________]
    Player player;
    Vector2 movement;
    bool mbIsCRPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.transform.position = startingPoint.transform.position;       // usage: 03, 
        /*auralight.transform.position = transform.position;
        flashlight.transform.position = handPoint.transform.position;*/
    }

    // Update is called once per frame
    // update controlls input , fixedUpdate controlls movement(physics)
    void Update()
    {
        // local variables
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        /*//2_GyungDo_light
        if (player.GetStatus() == EStatus.Sulre)
        {
            handleFlashlight();
            auralight.transform.position = transform.position;
            flashlight.transform.position = handPoint.transform.position;
        }*/


        // movement
        movement.x = horizontalInput;
        movement.y = verticalInput;
        if (player.mMP > 0 && player.mbMovable)
        {

            Vector2 input = new Vector2(horizontalInput, verticalInput);
            // 0 velocity
            player.Move(ref input, 1f);

            // if hit F key, invoke skill=iceshield
            // then player can't move untill others melt the ice
            // if hit F key, invoke skill + change img to ice filtered sprite
            // then set MP to 0 
            // !!!! need to check movement, it will move on next turn even if iced

        }

        /**
         * 2_GuyngDo
         * if chase&run phase is activated,
         * send signal back to gm to cotrol phase
         */
       /* if (mbIsCRPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gm.RunCRPhase();
            }
        }*/
        
    }
}
