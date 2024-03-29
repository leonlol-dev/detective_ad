using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    //Ink file
    [SerializeField] private TextAsset inkJSON;

    [SerializeField] private DialogueVoice voice;

    //Dialogue Manager
    private DialogueManager dialogueManager;

    public bool playerInRange;


    private void Awake()
    {
        playerInRange = false;
    }

    private void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    private void Update()
    {
        if(playerInRange && !dialogueManager.dialogueIsPlaying)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                dialogueManager.EnterDialogueMode(inkJSON, voice);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange= false;
        }
    }
}
