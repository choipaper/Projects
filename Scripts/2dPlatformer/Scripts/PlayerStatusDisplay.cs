using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{
    public GameObject playerStatusDisplayContainer;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public Text scores;
    public Player player;
    private int health;
    private int numOfHearts;


    private void Init()
    {
        health = player.Health;
        numOfHearts = player.NumOfHearts;
        hearts = new Image[numOfHearts];
        for (int i = 0; i < numOfHearts; i++)
        {
            hearts[i] = playerStatusDisplayContainer.transform.GetChild(i).GetComponent<Image>();
        }
    }

    void Update()
    {
        Init();
        //player.UpdateHealth();

        if(health > numOfHearts)
        {
            health = numOfHearts;
        }

        for(uint i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }                

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        scores.text = "Scores: " + player.Scores.ToString();
    }
}
