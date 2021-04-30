using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySystem : MonoBehaviour
{
    public const int SIZE = 6;
    public GameObject inventory;
    public GameObject[] itemSlots;
    private bool mbIsActive;

    // For Now: controls ItemController for all of items
    public ItemController itemController;

    private void Awake()
    {
        /*for(int i = 0; i < SIZE; i++)
        {
            itemSlots[i] = ;
        }*/
        //itemSlots = new GameObject[SIZE];
    }
    // Start is called before the first frame update
    void Start()
    {
        //itemSlots = new GameObject[SIZE];
        mbIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.I))
        {
            SetActivation(true);
        }*/

        inventory.SetActive(mbIsActive);
    }
    
    public void SetActivation(bool isActive)
    {
        mbIsActive = isActive;
    }

    public void TakeItem(Sprite item)
    {
        //Debug.Log(itemSlots[0].GetComponentInChildren<Image>().material);
        //itemSlots[0].GetComponentInChildren<Image>().enabled = true;
        //itemSlots[0].GetComponentInChildren<Image>().sprite = item; // this one also returns the one on the gameobject itself too
        itemSlots[0].transform.GetChild(0).GetComponent<Image>().sprite = item;
        
    }
}
