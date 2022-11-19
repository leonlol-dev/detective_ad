using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSceneController : MonoBehaviour
{
    public Animator[] animators;
    private bool pause;
    public bool cannotForward;

    private void Update()
    {
        //forward
        if(Input.GetKeyDown(KeyCode.E) && !cannotForward)
        {
            foreach (var animator in animators)
            {
                animator.SetFloat("timeMultiplier", 1);
            }
        }

        //backwards
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(cannotForward == true)
            {
                cannotForward = false;
            }

            foreach (var animator in animators)
            {
                animator.SetFloat("timeMultiplier", -1);
            }
        }

        //pause/play
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pause == true)
            {
                pause = false;
                foreach (var animator in animators)
                {
                    animator.SetFloat("timeMultiplier", 1);

                }
            }

            else
            {
                pause = true;

                foreach (var animator in animators)
                {
                    animator.SetFloat("timeMultiplier", 0);

                }
            }

            
        }
    }

    private void FixedUpdate()
    {
        foreach(Animator animator in animators)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName"))
            {
                animator.SetFloat("timeMultiplier", 0);
            }
        }
    }

    IEnumerator setAnimationThenReverse(Animator animator)
    {
        animator.SetBool("reverse", true);
        yield return new WaitForSeconds(0.33f);
        animator.SetBool("reverse", false);
    }
}
