using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    public Texture brushTexture;
    public Transform dot;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        /*if (Input.GetMouseButtonDown(0))
        {
            mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log(screenPos);
            //EventType.MouseDrag 
        }*/
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Instantiate(dot, screenPos, dot.rotation);
        }

    }

    /*private void OnGUI()
    {
        Event e = Event.current;

        if(!brushTexture)
        {
            Debug.LogError("Assign a Texture in the inspector.");
            return;
        }

        if(e.type == EventType.MouseDrag)
        {
            Debug.Log("Mouse is dragging");
            
        }
    }*/
}
