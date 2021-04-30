using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject player;
    //public Raycaster_TopDown ray;

    private Vector3 direction;
    //Raycaster_TopDown playerinfo;
    Vector2 myPos;

    // Start is called before the first frame update
    void Start()
    {
        myPos = transform.position;
        direction = getMousePos() - myPos;
        //playerinfo = player.GetComponent<Raycaster_TopDown>();
        SetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(direction * Time.deltaTime * 2.6f);
    }

    public void SetDirection()
    {
        //direction = playerinfo.GetBulletDirection();
    }

    Vector2 getMousePos()
    {
        Vector2 mousePos;
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //convert to screen position
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        return screenPos;
    }
}
