using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public GameObject timeText;
    public float maxTime = 180;      // 3 mins in seconds
    TimerState timer;
    long start,end;
    
    // Start is called before the first frame update
    void Start()
    {
        start = timer.start;
    }

    // Update is called once per frame
    void Update()
    {
        float minutes = Mathf.FloorToInt(maxTime / 60);
        float seconds = Mathf.FloorToInt(maxTime % 60);
        if (maxTime > 0)
        {
            maxTime -= Time.deltaTime;
            
            timeText.GetComponent<Text>().text = string.Format("TIMER\n{0:00}:{1:00}", minutes, seconds);
        }
        // Timer is done
        else
        {
            minutes = 0;
            seconds = 0;
        }
        
        //textInput.GetComponent<Text>().text = ;
        //textInput.GetComponent<TextField>().value = (timer.now - timer.start).ToString();
    }
}
