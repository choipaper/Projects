using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    //public GameObject obj;
    public Sprite[] changeableObj;// = new Sprite[5];
    // Update is called once per frame
    private Quaternion originalRotationValue;
    private int index = 0;
    private static int touchCount = 0;
    public CameraShake cameraShake;
    

    void Start()
    {
        //changeableObj = new Sprite[6];
        originalRotationValue = transform.rotation;
        //changeableObj = Resources.LoadAll<Sprite>("../Sprites/monitor_256px.png");
    }

    void Update()
    {
        Touch touch;
        float deg = 10.0f;
        //Debug.Log(changeableObj.Length);
        //touchCount = number of finger
        if (Input.touchCount>0)
        {
            
            touch = Input.GetTouch(0);
            //Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            
            if (touch.tapCount == 1)
            {
               
                //ShakingObj(.1f,deg);
                //ChangeImg();
            }
            Debug.Log(touchCount);
        }
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
            /*touchCount++;
            ShakingObj(.15f, deg);
            ChangeImg();*/
            //ShakingObj(.1f, -deg);
        }
    }
    private void OnMouseDown()
    {
        touchCount++;
        ShakingObj(.15f, 10);
        ChangeImg();
        
    }
    void ShakingObj(float duration, float deg)
    {
        
        float ogZ = transform.localRotation.z;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float z = Random.Range(-10f, 10f)*deg;
            //transform.Rotate(0f, 0f, 10, Space.Self);
            transform.Rotate(0.0f, 0.0f, -deg * Time.deltaTime, Space.World);
            //Debug.Log(Time.deltaTime);
            //
            elapsed += Time.deltaTime;
        }
        //transform.Rotate(0f, 0f, originalRotationValue.z);
    }

    void ChangeImg()
    {
        if(index < changeableObj.Length)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = changeableObj[index];
            //then delete obj
        }
       
        index += 1;
    }
}
