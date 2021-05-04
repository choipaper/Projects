using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDebug : MonoBehaviour
{
    public Player player;
    public GameObject DebugStatus;
    Text above;
    Text below;
    Text climbSlope;
    Text descSlope;
    Text slopeAng;
    Text speed;
    Text hitObj;
    Text faceDir;
    Text IsLanded;

    // Start is called before the first frame update
    void Start()
    {
        above = DebugStatus.transform.GetChild(0).transform.GetComponent<Text>();
        below = DebugStatus.transform.GetChild(1).transform.GetComponent<Text>();
        climbSlope = DebugStatus.transform.GetChild(2).transform.GetComponent<Text>();
        descSlope = DebugStatus.transform.GetChild(3).transform.GetComponent<Text>();
        slopeAng = DebugStatus.transform.GetChild(4).transform.GetComponent<Text>();
        speed = DebugStatus.transform.GetChild(5).transform.GetComponent<Text>();
        hitObj = DebugStatus.transform.GetChild(6).transform.GetComponent<Text>();
        faceDir = DebugStatus.transform.GetChild(7).transform.GetComponent<Text>();
        IsLanded = DebugStatus.transform.GetChild(8).transform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        updateStatus();
    }

    void updateStatus()
    {
        above.text ="Above: " + player.controller.collisions.above;
        below.text = "Below: " + player.controller.collisions.below;
        climbSlope.text = "climbSlope: " + player.controller.collisions.climbingSlope;
        descSlope.text = "descSlope: " + player.controller.collisions.descendingSlope;
        slopeAng.text = "slopeAng: " + player.controller.collisions.slopeAngle;
        speed.text = "speed: " + player.velocity.x;
        hitObj.text = "hitObj: " + player.controller.collisions.hitTag;
        faceDir.text = "faceDir: " + player.controller.collisions.faceDir;
        IsLanded.text = "IsLanded: " + player.bLanded;
    }
}
