using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    InputManager input; 
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;

    void Awake(){
        input = new InputManager();

        // input.Player.Move.performed += ctx => {
        //     currentMovement = ctx.ReadVaue<Vector2>();
        //     movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        // };
        input.Player.Move.performed += ctx => {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        // ---------------------------------
        input.Player.Move.canceled += ctx => movementPressed = false;
        input.Player.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

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
        handleMovement();
        handleRotation();
        
    }

    void handleRotation(){
        // current pos of character
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    void handleMovement(){
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        // Checking for walking
        if(movementPressed && !isWalking){
            animator.SetBool(isWalkingHash, true);
        }

        if(movementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }

        // Checking for running 
        // if player presses move and run and ISNT already running
        if((movementPressed && runPressed) && !isRunning){
            // set running to true
            animator.SetBool(isRunningHash, true);
        }

        // if not pressing move or not run and we ARE already running
        if((!movementPressed || !runPressed) && isRunning){
            // set running to false
            animator.SetBool(isRunningHash,false);
        }
    }

    void onEnable(){
        input.Player.Enable();
    }

    void OnDisable(){
        input.Player.Disable();
    }
}
