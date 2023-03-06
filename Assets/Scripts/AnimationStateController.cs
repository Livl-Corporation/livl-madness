using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    
    private Animator animator;
    int isWalkingHash;
    private int isRunningHash;
    private int isScanningHash;
    
    void Start()
    {
        animator = character.GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isScanningHash = Animator.StringToHash("isScanning");
    }

    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey(KeyCode.Z);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isScanningPressed = Input.GetKey(KeyCode.Space);
        bool isScanning = animator.GetBool(isScanningHash);
        
        bool isKeyReleased = Input.GetKeyUp(KeyCode.Z);
        bool isScanningReleased = Input.GetKeyUp(KeyCode.Space);

        if(!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        } 
        
        if(isKeyReleased && (isWalking && !forwardPressed))
        {
            animator.SetBool(isWalkingHash, false);
        }
        
        if(!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        
        if(isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
        
        if(isScanningPressed && !isScanning)
        {
            animator.SetBool(isScanningHash, true);
        }
        if(isScanningReleased && (isScanning && !isScanningPressed))
        {
            animator.SetBool(isScanningHash, false);
        }
    }
}
