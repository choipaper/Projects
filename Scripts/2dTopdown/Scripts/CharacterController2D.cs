using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float speed;
    public GameObject flashLight;

    // previous mouse x and y position
    private float prevMX, prevMY;
    
    // Start is called before the first frame update
    void Start()
    {
        prevMX = Input.GetAxis("Mouse X");
        prevMY = Input.GetAxis("Mouse Y");

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Debug.Log(prevMX + " , " + prevMY);
        //FollowMouse();
    }

    private void FixedUpdate()
    {
        FollowMouse();
    }
    // Move
    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

    }
    // Follow mouse
    public void FollowMouse()
    {
        //Vector3(Input.GetAxis("Mouse X"), 0) 
        float currMX = Input.GetAxis("Mouse X");
        float currMY = Input.GetAxis("Mouse Y");
        float diffMX = currMX - prevMX;
        float diffMY = currMY - prevMY;
        float changedDeg = Mathf.Atan((diffMY / diffMX) * Mathf.Rad2Deg);

       // Debug.Log(changedDeg);

        // rotate it by amount of changed angle
        //flashLight.transform.Rotate(Vector3.forward,changedDeg,Space.World);

        //other method
        Vector3 to = new Vector3(0, 0, changedDeg);
        //flashLight.transform.eulerAngles = Vector3.Lerp(flashLight.transform.rotation.eulerAngles, to, Time.deltaTime); //error

        prevMX = currMX;
        prevMY = currMY;
        //currMX = 0f;
        //currMY = 0f;
    }

    // Collision
    // attributes - polic or thief
    // police's flash light movement by mouse position
    // theif's skill transform
}
