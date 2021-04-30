using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eLevels
{
    LEVEL01 = 15,
    LEVEL02 = 12,
    LEVEL03 = 13
}

public class GameManager : MonoBehaviour
{
    //private int mCurrentScene = 0;
    public GameObject[] items;
    public Player player;
    public UIManager uiManager;

    void Start()
    {
        player.InitItems(ref items);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }

}
