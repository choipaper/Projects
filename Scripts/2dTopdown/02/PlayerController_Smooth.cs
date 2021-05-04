using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (Player))]
public class PlayerController_Smooth : MonoBehaviour
{
    //[__________Public__________] 
    public GameObject startingPoint;
    public GameObject handPoint;
    public float moveSpeed = 0.3f;
    //lights
    public GameObject auralight;
    public GameObject flashlight;
    //Game manager
    public GameManager gm;

    //Inventory
    public InventorySystem inventory;

    public float testDeg;
    public float rotationSpeed;
    //[__________Private__________]
    Player player;
    Vector2 movement;
    bool mbIsCRPhase;
    bool mbIsFlashOn;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.transform.position = startingPoint.transform.position;       // usage: 03, 
        auralight.transform.position = transform.position;
        flashlight.transform.position = handPoint.transform.position;
        mbIsCRPhase = false;
        mbIsFlashOn = true;
    }

    // Update is called once per frame
    // update controlls input , fixedUpdate controlls movement(physics)
    void Update()
    {
        // local variables
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //2_GyungDo_light
        if(player.GetStatus() == EStatus.Sulre)
        {
            handleFlashlight();
            auralight.transform.position = transform.position;
            flashlight.transform.position = handPoint.transform.position;
        }


        // movement
        movement.x = horizontalInput;
        movement.y = verticalInput;
        if (player.mMP > 0 && player.mbMovable)
        {
            //player.Move(new Vector2(horizontalInput, verticalInput));

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
        if(mbIsCRPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gm.RunCRPhase();
            }
        }
        /*// just for testing
        if (isIced)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = iceImg;
            mbMovable = false;
        }*/

        //Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I pressed");
            inventory.SetActivation(true);
        }

        // On/Off flashlight
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("FFFF");
            if(!mbIsFlashOn)
            {
                TurnOnOffLight(true);
            }
            else
            {
                TurnOnOffLight(false);
            }
            mbIsFlashOn = !mbIsFlashOn;
        }

        //Get Item
        if(Input.GetKeyDown(KeyCode.E))
        {
            // put it into inventory
            // Mechanism: just copy sprite paste on the Inventory's itemslot, then, configure the real object
            // How? inactive the obj and put all of info of the obj under player
            // For now: just copy, paste and disable
            // inventory.TakeItem(gameobject);
        }
    }

    void FixedUpdate()
    {
        player.Move(ref movement,  moveSpeed * Time.fixedDeltaTime);
    }

    void TurnOnOffLight(bool onOff)
    {
        flashlight.SetActive(onOff);
    }

    void handleFlashlight()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        // why screenPos.x - handpoint.position.x? not perfectly follow where I clicked or the green ray beacuse? 
        Ray2D ray2Mouse = new Ray2D(new Vector2(handPoint.transform.position.x, handPoint.transform.position.y), new Vector2(screenPos.x - handPoint.transform.position.x, screenPos.y - handPoint.transform.position.y));
        //flashlight.transform.rotation = ;
        Debug.DrawRay(ray2Mouse.origin, ray2Mouse.direction, Color.green);
        //Debug.Log(ray2Mouse.direction);
        /*if (ray2Mouse.direction.x < 0)
        {
            deg = Mathf.Rad2Deg * (Mathf.Atan(ray2Mouse.direction.x / ray2Mouse.direction.y));
        }
        else
        {
            deg = Mathf.Rad2Deg * Mathf.Atan(ray2Mouse.direction.y / ray2Mouse.direction.x);
        }*/
        // other method
        // calculate angle between two lines
        /*if(oldMousPos != mousePos)
        {
            Vector2 a = new Vector2(mousePos.x - flashlight.transform.position.x, mousePos.y - flashlight.transform.position.y);
            Vector2 b = new Vector2(oldMousPos.x - flashlight.transform.position.x, oldMousPos.y - flashlight.transform.position.y);
            
            //angle
            deg = Vector2.Angle(b, a);
            //flashlight.transform.eulerAngles = Vector3.Lerp(flashlight.transform.rotation.eulerAngles, new Vector3(0, 0, deg), Time.deltaTime);
            oldMousPos = mousePos;
            //Debug.Log(Mathf.Rad2Deg * deg);
        }*/
        Vector2 direction = new Vector2(screenPos.x - transform.position.x, screenPos.y - transform.position.y).normalized;
        float test = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        flashlight.transform.rotation = Quaternion.Euler(Vector3.back * (-test + 90f));

    }

    public void SetPhase()
    {
        mbIsCRPhase = true;
    }

    private void OnMouseDown()
    {
        // 2_GyungDo
        // clicking on other players(thief) leads to catching stage(panel)
        // when clicked check what player's status (like thief or police)
        // if thief, send signal to game manager to arrange chase&run game panel
        //gm.StartChaseRunPhase();
    }

   /* private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Ray2D rays = new Ray2D(screenPos, Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rays.origin, rays.direction, 1f, LayerMask.NameToLayer("Items"));
            if (hit)
            {
                inventory.TakeItem(hit.collider.gameObject);
                Debug.Log(hit.collider.name);
            }

            Debug.DrawRay(rays.origin, rays.direction, Color.red);
        }
    }*/

    public void SetStatus(EStatus status)
    {
        player.SetStatus(status);
    }



}
