using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    //Mouse
    [SerializeField] private PlayerMove playerMovement;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private float textSpeed = 0.25f;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private Coroutine anim;

    public bool dialogueIsPlaying{ get; private set; }
     
    private void Start()
    {

        ExitDialogueMode();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        // If you are looking for the mouse lock feature,
        // save yourself the headache and look for the Player Camera script.

        if(!dialogueIsPlaying)
        {
            playerMovement.canMove = true;
            return;
            
        }

        playerMovement.canMove = false;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }

    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

    }

    private IEnumerator PlayTextAnimation(string text)
    {
        // This plays animation like a typing animation.
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);   
        } 
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        //Stops the coroutine animation, if player skips dialogue.
        if (dialogueText.text.Length < currentStory.currentText.Length)
        {
            StopCoroutine(anim);
        }

        //Clears the text box contents and starts to play animation for the current
        //story and displays choices if needs.
        if (currentStory.canContinue)
        {
            dialogueText.text = "";
            //dialogueText.text = currentStory.Continue();
            anim = StartCoroutine(PlayTextAnimation(currentStory.Continue()));
            DisplayChoices();
        }

        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //Error check
        if(currentChoices.Count > choices.Length) { Debug.Log("Too many Choices for UI to support."); }

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event system requires we clear it first.
        // For at least one frame before we set it as current object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }



    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
