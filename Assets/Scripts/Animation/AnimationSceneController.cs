using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSceneController : MonoBehaviour
{
    public Animator[] animators;
    private bool pause;
    private bool forward;
    public bool cannotForward; //this should be renamed to play but oh well

    private void Start()
    {
        forward = true;
    }

    private void Update()
    {
        //forward
        if(Input.GetKeyDown(KeyCode.E) && !cannotForward)
        {
            foreach (var animator in animators)
            {
                animator.SetFloat("timeMultiplier", 1);
                
            }

            forward = true;
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

            forward = false;
        }

        //pause/play
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //play
            if (pause == true)
            {
                pause = false;

                if(forward)
                {
                    foreach (var animator in animators)
                    {
                        animator.SetFloat("timeMultiplier", 1);

                    }
                }

                else if (!forward)
                {
                    foreach (var animator in animators)
                    {
                        animator.SetFloat("timeMultiplier", -1);

                    }
                }
                
            }


            //pause
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

//NOTES
//IF I WANT MULTIPLE ITEMS WITH TIME/ANIMATION MULTIPLATION I NEED TO ADD ANIMATION EVENTS TO ALL ANIMATION FILES.
