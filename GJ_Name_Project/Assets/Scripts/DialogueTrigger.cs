using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public Player player;


    public int idSign;
    public bool isSkelletonBossDefeat;
    public bool wasShowedSkelletonBoss;

    public bool wasShowedDash;
    
    
    public bool wasShowedFinalMessage;

    
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isSkelletonBossDefeat && !wasShowedSkelletonBoss && idSign == 0)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                wasShowedSkelletonBoss = true;
            }


            if (!wasShowedDash && idSign == 1)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                wasShowedDash = true;
                FindObjectOfType<Player>().isUnlockedDash = true;
                //player.isUnlockedDash = true;
            }

            if (!wasShowedFinalMessage && idSign == 2)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                wasShowedFinalMessage = true;                                
            }


        }
    }

}
