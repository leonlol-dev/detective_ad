using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRealm : MonoBehaviour
{
    public Camera Normal;
    public Camera Realm;

    public GameObject normalVolume;
    public GameObject realmVolume;

    private Animator blinkControl;

    // Start is called before the first frame update
    void Start()
    {
        Normal.enabled = true; 
        Realm.enabled = false;

        normalVolume.SetActive(true);
        realmVolume.SetActive(false);

        blinkControl = GetComponent<Animator>();
    }

    void Vision()
    {
        Normal.enabled = !Normal.enabled;
        Realm.enabled = !Realm.enabled;

        normalVolume.SetActive(!normalVolume.activeSelf);
        realmVolume.SetActive(!realmVolume.activeSelf);

        blinkControl.SetBool("Blink", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            blinkControl.SetBool("Blink", true);
        }
    }

}
