﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController2D : RaycastController
{
	public LayerMask passengerMask;

	public Vector3[] localWaypoints;
	Vector3[] globalWaypoints;

	public float speed;
	public float waitTime;
	[Range(0, 2)]
	public float easeAmount;

	int fromWaypointIndex;
	float percentBetweenWaypoints;
	float nextMoveTime;

	private int faceDir;

	public override void Start()
	{
		base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}

	void Update()
	{
		UpdateRaycastOrigins();
		Vector3 velocity = CalculatePlatformMovement();
		// Fliping sprite - temporary
		faceDir = (int)Mathf.Sign(velocity.x);
		// to left
		if(velocity.x < 0 && faceDir == -1)
        {
			Flip();
        }
		// to right
		else if(velocity.x > 0 && faceDir == 1)
        {
			Flip();
        }
		transform.Translate(velocity);
	}

	float Ease(float x)
	{
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}

    Vector3 CalculatePlatformMovement()
    {

        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);
		
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

	// Fliping sprite - temporary
	// this logic is different from what I implenemted in controller2D for player
	// It is opposite to that - why?
	private void Flip()
	{
		Vector3 scale = transform.localScale;
		if (faceDir == 1)
		{
			if (scale.x > 0)
			{
				scale.x *= -1;
			}

		}
		else if (faceDir == -1)
		{
			scale.x = Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
	}

	void OnDrawGizmos()
	{
		if (localWaypoints != null)
		{
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
}
