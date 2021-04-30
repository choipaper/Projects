using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public Player player;
    public GameObject npc;
    private GameObject dialogBox;

    private void Start()
    {
        dialogBox = gameObject.transform.Find("NPC_Dialog").gameObject;
    }
    private void Update()
    {
        if(FoundPlayer())
        {
            dialogBox.SetActive(true);
            npc.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Return) && FoundPlayer())
        {
            TriggerDialogue();
        }
        else if(!FoundPlayer())
        {
            dialogBox.SetActive(false);
            npc.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        
    }

    private bool FoundPlayer()
    {
        if((npc.transform.position.x - 16f >= player.transform.position.x) || (npc.transform.position.x + 16f >= player.transform.position.x ))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
