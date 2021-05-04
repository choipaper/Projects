﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinWidth = 0.015f;
    const float minDistnBtwRays = 0.25f;

    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;
    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    [HideInInspector]
    public BoxCollider2D boxCollider;

    public RaycastOrigins raycastOrigins;

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    // Update Raycast origins 
    // by using bounds(vector2(x,y)) of obj to set raycast
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        // Shrink bonds little bit to detect other objs from the surface of the obj
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Calculate ray spacing by the object's size 
    public void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        // Get the number of rays(int) by total x or y / minimum distance between rays
        horizontalRayCount = Mathf.RoundToInt(boundsHeight / minDistnBtwRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / minDistnBtwRays);

        // Get the actual spacing between rays by number of rays 
        //actual size of object(x,y) divided by total number of rays - 1(since spacing is beween two rays) 
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
