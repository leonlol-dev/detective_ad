using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVoice : MonoBehaviour
{

    //For every number of characters play a sound. 
    public int soundFrequency = 2;

    //Minimum and maximum pitch, the sound will play at a pitch between these two values.
    [Range(-3, 3)]
    public float minPitch = 0.5f;
    [Range(-3, 3)]
    public float maxPitch = 3f;

    public AudioClip[] clips;
}
