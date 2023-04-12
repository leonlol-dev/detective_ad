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

    //UI
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    //Audio
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private bool stopAudioSource;

    //Audio clips
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip dialogueAudioClip;

    //For every number of characters play a sound. 
    [SerializeField] private int soundFrequency = 2;

    //Pitch
    [Range(-3, 3)]
    [SerializeField] private float minPitch = 0.5f;
    [Range(-3, 3)]
    [SerializeField] private float maxPitch = 3f;

    //Player
    [SerializeField] private PlayerObjectInspect playerInspect;


    //Default Audio
    //(if the character has not set up a voice script it will default to this.)
    [SerializeField] private AudioClip[] defaultClips;
    [SerializeField] private int defaultSoundFrequency = 2;
    [Range(-3, 3)]
    [SerializeField] private float defaultMinPitch = 0.5f;
    [Range(-3, 3)]
    [SerializeField] private float defaultMaxPitch = 3f;

    //Animation
    [SerializeField] private float textSpeed = 0.25f;

    //Raycast 
    [SerializeField] private Camera cam;
    [SerializeField] private float rayCastRange = 2.0f;


    //Private variables
    private Story currentStory;
    private Coroutine anim;
    

    public bool dialogueIsPlaying{ get; private set; }
     
    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
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
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayCastRange))
        {

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitDialogueMode();
        }


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

    public void EnterDialogueMode(TextAsset inkJSON, DialogueVoice voice)
    {
        if(voice != null)
        {
            setVoice(voice);   
        }

        else
        {
            setDefaultVoice();
        }

        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

    }



    private void ExitDialogueMode()
    {
        audioSource.Stop();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        audioSource.Stop();
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

    private IEnumerator PlayTextAnimation(string text)
    {
        // This plays animation like a typing animation.
        foreach (char c in text)
        {
            PlayDialogueSound(c);
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void PlayDialogueSound(int displayedTextAmount)
    {
        if(displayedTextAmount % soundFrequency == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clips[Random.RandomRange(0, clips.Length)]);
        } 
    }

    public void setVoice(AudioClip[] _clips, int _soundFrequency, int _minPitch, int _maxPitch)
    {
        clips = _clips;
        soundFrequency = _soundFrequency;
        minPitch = _minPitch;
        maxPitch = _maxPitch;
    }

    public void setVoice (DialogueVoice _voice)
    {
        clips = _voice.clips;
        soundFrequency = _voice.soundFrequency;
        minPitch = _voice.minPitch;
        maxPitch = _voice.maxPitch;
    }

    private void setDefaultVoice()
    {
        clips = defaultClips;
        soundFrequency = defaultSoundFrequency;
        minPitch = defaultMinPitch;
        maxPitch = defaultMaxPitch;

    }
}
