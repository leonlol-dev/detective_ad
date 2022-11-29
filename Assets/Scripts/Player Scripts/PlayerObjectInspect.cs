using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectInspect : MonoBehaviour
{
    public Camera cam;
    public float interactRange;
    public List<GameObject> worldEvidenceFolder;
    public List<AudioClip> audioClips;

    private GameObject player;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        audioSource = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange))
        {
            if(hit.collider.gameObject.tag == "WorldEvidence")
            {
                GameObject evidence = hit.collider.gameObject;

                if(worldEvidenceFolder.Contains(hit.collider.gameObject))
                {
                    //do nothing
                    return;
                }
                else
                {
                    worldEvidenceFolder.Add(evidence);
                    PlayInteractionSpeech(evidence.GetComponent<WorldEvidenceInteractSound>().clip);
                }

            }
        }
    }

    void PlayInteractionSpeech(AudioClip _clip)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(_clip);
        }
        
    }
}
