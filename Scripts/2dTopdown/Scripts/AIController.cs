using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * - move around on selected point
 * - follow a line
 * - findPlayer
 * - AttackPlayer
 * - TakeDamage
 * *******************
 * PUBLIC FUNCTIONS
 * *******************
 * - TakeDamage()
 * -
 * *******************
 * PRIVATE FUNCTIONS
 * *******************
 * - followLine
 * - findPlayer
 */
[RequireComponent(typeof(Character2DBase))]
public class AIController : MonoBehaviour
{
    public Vector3[] waypoints;

	Vector3[] globalWaypoints;
	Character2DBase characterBase;

	// Start is called before the first frame update
    void Start()
    {
		characterBase = GetComponent<Character2DBase>();
		globalWaypoints = new Vector3[waypoints.Length];
		for (int i = 0; i < waypoints.Length; i++)
		{
			globalWaypoints[i] = waypoints[i] + transform.position;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if(transform.position.x <= globalWaypoints[0].x)
        {
			Debug.Log(globalWaypoints[0].normalized);
			Vector2 dir = globalWaypoints[0] - transform.position;
			transform.Translate(dir * Time.deltaTime * 1.3f);
		}
        else
        {
			transform.Translate(globalWaypoints[1].normalized * Time.deltaTime * 1.3f);
		}

    }

	void OnDrawGizmos()
	{
		if (waypoints != null)
		{
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < waypoints.Length; i++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : waypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
}
