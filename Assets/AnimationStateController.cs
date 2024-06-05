using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        

        // if player is pressing W
        if(!isWalking && forwardPressed){
            // set the isWalking bool in the animator to true
            animator.SetBool(isWalkingHash, true);
        }
        
        // if player isn't pressing W
        if(isWalking && !forwardPressed){
            // set the isWalking bool in the animator to false
            animator.SetBool(isWalkingHash, false);
        }
        
        // if player isn't already running AND is walking and presses left shift
        if(!isRunning && (forwardPressed && runPressed)){
            // set the isRunning bool to be true (in the animator)
            animator.SetBool(isRunningHash, true); 
        }

        // if the player is already running AND stops running or walking
        if(isRunning && (!forwardPressed || !runPressed)){
            animator.SetBool(isRunningHash, false);
        }
    }
}
