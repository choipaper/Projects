using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public InventorySystem inventory;
    public Items item;
    // Start is called before the first frame update
    /*private void Awake()
    {
        item = GetComponent<Items>();
    }*/
    void Start()
    {
        //item = GetComponent<Items>();
    }

    // Update is called once per frame
    void Update()
    {
        item.Init();
        //while running, check if one of items was clicked then send a signal to inventorySystem
        if(item.IsSelected())
        {
            inventory.TakeItem(item.GetSprite());
        }
    }

    public void DisableItem()
    {
        gameObject.SetActive(false);
    }
   
}
