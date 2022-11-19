using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pete : MonoBehaviour
{
    private Animator animator;
    public AnimationSceneController animController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        Debug.Log("pause");
        animator.SetFloat("timeMultiplier", 0);
        animator.Play("Dying", 0, (1f / 146) * 125);
        animController.cannotForward = true;
    }
}
