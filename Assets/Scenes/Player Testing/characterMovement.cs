using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    InputManager input;     // variable for storing the instance of InputManager
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;


    void Awake(){
        input = new InputManager();

        //-----------------------------------------
        // input.Player.Move.performed += ctx => Debug.Log(ctx.ReadValueAsObject());
        // ----------------------------------------
        //input.Player.Move.canceled += walkPerformed;

        input.Player.Move.started += ctx => Debug.Log("STARTED");
        input.Player.Move.performed += ctx => {
            //Debug.Log(".performed: " + ctx.ReadValueAsObject());
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        // input.Player.Move.canceled += ctx => {
        //     movementPressed = false;
        //     currentMovement = Vector2.zero;
        // };
        
        input.Player.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    // private void walkPerformed(InputAction.CallbackContext context){
    //     currentMovement = context.ReadValue<Vector2>();
    //     Debug.Log(currentMovement);
    //     movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    // }

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
        handleRotation();
        handleMovement();
    }

    void handleRotation(){
        // current position of character
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;
        
        Debug.Log(currentMovement);

        transform.LookAt(positionToLookAt);
    }

    void handleMovement(){
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        // ------------------------------------- walking -------------------------------------
        // start walking if movementPressed is true and the player isn't already walking
        if(movementPressed && !isWalking){
            animator.SetBool(isWalkingHash, true);
        }
        // stop walking if movementPressed is false and the player's already walking
        if(!movementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }

        // ------------------------------------- running ------------------------------------- 
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
