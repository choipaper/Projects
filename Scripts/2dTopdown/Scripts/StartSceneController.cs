using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject optionPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // Load findgame scene
    public void LoadGameScene(int index)
    {
        // load find game scene 2
        SceneManager.LoadScene(index);

    }

    // Enable Option panel - may not be needed
    public void EnableOptionMenu()
    {
        optionPanel.SetActive(true);
    }

    // Setting for volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    //Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
