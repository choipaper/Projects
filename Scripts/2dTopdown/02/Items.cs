using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    // Public
    // Private
    bool mbIsSelected;
    SpriteRenderer mSpriteRenderer;


    // Start is called before the first frame update
    void Awake()
    {
        mbIsSelected = false;
        mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.name);
    }

    /**
     * 
     * Initial placement on the certain area by randomly placed
     * need: where is the AREA
     */
    public void Init()
    {

    }

    public void SetSelected(bool isSelected)
    {
        mbIsSelected = isSelected;
    }

    public bool IsSelected()
    {
        return mbIsSelected;
    }

    public Sprite GetSprite()
    {
        //Debug.Log(mSpriteRenderer.sprite.name);
        return mSpriteRenderer.sprite;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Ray2D rays = new Ray2D(screenPos, Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rays.origin, rays.direction, 1f);
            if (hit)
            {
                Debug.Log(hit.collider.name + " Founded!");
                // send signal to where?? to Inventory
                SetSelected(true);
            }

            Debug.DrawRay(rays.origin, rays.direction, Color.red);
        }
    }
}
