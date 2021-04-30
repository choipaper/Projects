using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene01Mngr : MonoBehaviour
{
    // Global var
    const int MAXNUM = 6;
    // public 
    public Sprite[] mpSprite = new Sprite[2];
    public Image[] mpImgs = new Image[MAXNUM];
    public GameObject mpDisplayPanel;
    public MiniFigController player;
    //private


    // Start is called before the first frame update
    void Start()
    {
        updateMPdisplay(0);
    }

    // Update is called once per frame
    void Update()
    {
        updateMPdisplay(player.GetMP());
    }

    // setting up to display MP 1line(6 MP available)
    void updateMPdisplay(int mp)
    {
        for(int i = 0; i < mpImgs.Length; i++)
        {
            if(mp > 0)
            {
                // filled mp img
                mpImgs[i].sprite = mpSprite[1];
                mp--;
            }
            else
            {
                // empty mp img
                mpImgs[i].sprite = mpSprite[0];
            }
        }
    }
}
