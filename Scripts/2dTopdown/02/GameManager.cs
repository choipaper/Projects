using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CRPanel;
    public GameObject policeMal;
    public GameObject policeImg;
    public PlayerController_Smooth playerController;

    // Private
    bool mbIsCRPhase = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    /** 
     * 2_GyungDo
     * arrange chase&run phase
     */
    public void StartChaseRunPhase()
    {
        CRPanel.SetActive(true);
        mbIsCRPhase = true;

        //send signal back to player
        playerController.SetPhase();
        
    }

    public void RunCRPhase()
    {
        policeImg.GetComponent<RectTransform>().Translate(Vector3.right);
        policeMal.GetComponent<RectTransform>().Translate(Vector3.right);
        Debug.Log("Space Bar!");
    }

    /**
     * 3_Crossing Dark Bridge
     * 
     */
    // Set Player's status 
    public bool SetPlayerStatus(int statusNo, PlayerController_Smooth p)
    {
        p.SetStatus((EStatus)statusNo);
        return false;
    }

    // if player.mStatus = Spectator
    // Choose 3 tiles and Show 3 tiles for 60s
    public void Choose3Tiles(Vector2 tilePos, int ticket)
    {
        if(ticket <= 3)
        {
            //show the tile

        }
    }
}
