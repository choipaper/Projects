using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text statText;
    public Text DebugText;
    public GameObject gameOverPanel;
    public GameObject completePanel;
    public Player player;

    // Update is called once per frame
    void Update()
    {
        showStats();
        if(player.numOfSteps <= 0 && !player.bIsArrived)
        {
            gameOverPanel.SetActive(true);
        }
        if(player.bIsArrived)
        {
            completePanel.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void showStats()
    {
        statText.text = "STEPS: " + player.numOfSteps.ToString();
    }
}
