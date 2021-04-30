using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Opacity controller component
 * When Players are nearby, This obejct's opacity comes down to their aimOpacity
 */
public class OpacityController : MonoBehaviour
{
    public LayerMask playerLayer;
    public float alphaValue;

    BoxCollider2D collider;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray2D rays = new Ray2D(transform.position, Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, playerLayer);
        if(hit)
        {
            //mat.a = alphaValue;
            changeColor(alphaValue);

        }
        else
        {
            changeColor(1f);
        }
        Debug.DrawRay(transform.position, Vector2.right);
        
    }

    void changeColor(float alpha)
    {
        Color oldColor = sr.color;
        Color newColor = new Color(1f,1,1, alpha);
        sr.color = newColor;
        //Debug.Log("change Color");
    }
}
