using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DebugPanelManager : MonoBehaviour
{
    public GameObject player;
    //public MiniFigController playerT;
    public Raycaster_TopDown raycaster;
    
    // private
    const float SIZE = 100f;
    // create each object to make a panel
    GameObject debugDisplay;
    GameObject debugTxt;
    Text txt;
    // Start is called before the first frame update
    void Start()
    {
        createDebugDisplayPanel();
    }

    // Update is called once per frame
    void Update()
    {
        // update panel text
        //updateDebugPanel();
        showRaycaseterDebugInfo();
    }

    void createDebugDisplayPanel()
    {
        debugDisplay = new GameObject("DebugDisplayPanel");
        debugTxt = new GameObject("debugTxt");
        // set panel under the current gameobject(canvas)
        debugDisplay.transform.SetParent(this.transform);
        debugTxt.transform.SetParent(debugDisplay.transform);

        //set debugTxt to fit debug display
        RectTransform txtRT = debugTxt.AddComponent<RectTransform>();
        CanvasRenderer txtCR = debugTxt.AddComponent<CanvasRenderer>();

        txtRT.anchorMin = new Vector2(0f, 0f);
        txtRT.anchorMax = new Vector2(1f, 1f);
        txtRT.sizeDelta = new Vector2(1f, 1f);        //sizedelta ratio by parents'obj's size in here 1:1 scale;
        
        //text
        txt = debugTxt.AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.alignment = TextAnchor.UpperLeft;
        txt.text = "Queue: " + 3 + '\n';
        txt.text += "This is next line! \n";

        // building up DebugDisplay panel
        RectTransform rt = debugDisplay.AddComponent<RectTransform>() as RectTransform;
        CanvasRenderer cr = debugDisplay.AddComponent<CanvasRenderer>() as CanvasRenderer;
        Image img = debugDisplay.AddComponent<Image>() as Image;
        rt.sizeDelta = new Vector2(2 * SIZE, SIZE);        // unlike txtRT this congfigs width and height of debugDisplay
        img.color = Color.black;

        //set anchor to top right
        rt.anchorMin = new Vector2(1f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.localPosition = gameObject.GetComponent<RectTransform>().localPosition - new Vector3((rt.rect.width / 2), (rt.rect.height / 2), 0);
    }
    
    private void showRaycaseterDebugInfo()
    {
        /**
         * E:
         * W:
         * S:
         * N:
         * 
         */
        debugTxt.GetComponent<Text>().text = "Collision: " + '\n';                            // add player queue
        debugTxt.GetComponent<Text>().text += "E: " + raycaster.mbIsCollided_E + '\n';
        debugTxt.GetComponent<Text>().text += "W: " + raycaster.mbIsCollided_W + '\n';
        debugTxt.GetComponent<Text>().text += "S: " + raycaster.mbIsCollided_S + '\n';
        debugTxt.GetComponent<Text>().text += "N: " + raycaster.mbIsCollided_N + '\n';

    }
    private void updateDebugPanel()
    {
        /**
         * Queue:
         * Player ID:
         * MP:
         * Using skill
         * 
         */
        debugTxt.GetComponent<Text>().text = "Queue: " + '\n';                            // add player queue
        debugTxt.GetComponent<Text>().text += "Player ID: " + player.name + '\n';
        //debugTxt.GetComponent<Text>().text += "MP: " + playerT.GetMP() + '\n';
        debugTxt.GetComponent<Text>().text += "Using Skill: " + '\n'; ;                   // show using skill

    }
}
