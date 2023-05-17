using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float AnimBlendSpeed = 8.9f;
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 70f;
    [SerializeField] private float MouseSensitivity = 21.9f;
    [SerializeField, Range(10, 500)] private float JumpFactor = 150f;
    [SerializeField] private float Dis2Ground = 0.8f;
    [SerializeField] private LayerMask GroundCheck;
    [SerializeField] private float AirResistance = 0.8f;
    
    //[Tooltip("Acceleration and deceleration")]
    //[SerializeField] private float SpeedChangeRate = 10.0f;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip LandingAudioClip;
    [SerializeField] public AudioClip[] FootstepAudioClips;
    [SerializeField] private AudioClip IOSNotificationMessageClip;
    
    [Header("Animation Rigging")]
    [SerializeField] private Rig AimRig;
    [SerializeField] private float AimRigWeight;
    
    [Header("Aiming Settings")]
    [SerializeField] private Transform AimPosition;
    [SerializeField] private float AimSmoothSpeed = 20;
    [SerializeField] LayerMask AimLayerMask;
    [SerializeField] private Camera PlayerCamera;
    
    private Rigidbody _playerRigidbody;
    private InputManager _inputManager;
    private Animator _animator;
    private bool _grounded = false;
    private bool _hasAnimator;
    private int _xVelHash;
    private int _yVelHash;
    private int _jumpHash;
    private int _groundHash;
    private int _fallingHash;
    private int _aimingHash;
    private int _zVelHash;
    private int _crouchHash;
    private float _xRotation;

    private const float _walkSpeed = 2f;
    private const float _runSpeed = 6f;
    private Vector2 _currentVelocity;
    
    private Player _player;

    private void Start()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        
        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _zVelHash = Animator.StringToHash("Z_Velocity");
        _jumpHash = Animator.StringToHash("Jump");
        _groundHash = Animator.StringToHash("Grounded");
        _fallingHash = Animator.StringToHash("Falling");
        _crouchHash = Animator.StringToHash("Crouch");
        _aimingHash = Animator.StringToHash("Aiming");
        
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        HideCursor();
    }

    private void HideCursor()
    {
        if (PlayerUI.isPaused || !PlayerUI.isCursorCaptured)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor to be able to click on the UI
            }

            // Reset the animator values to put the player to idle state (not moving)
            _animator.SetFloat(_xVelHash , 0);
            _animator.SetFloat(_yVelHash, 0);
            _animator.SetFloat(_zVelHash, 0);

            return;
        } 

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void FixedUpdate() {
        SampleGround();
        Move();
        HandleJump();
        HandleCrouch();
        HandleAiming();
        ManageAimingRig();
    }

    private void ManageAimingRig()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = PlayerCamera.ScreenPointToRay(screenCenter);
        
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, AimLayerMask))
        {
            AimPosition.position = Vector3.Lerp(AimPosition.position, hit.point, AimSmoothSpeed * Time.deltaTime);        
        }
    }
    
    private void LateUpdate() {
        CamMovements();
    }

    private void Move()
    {
        if(PlayerUI.isPaused || !PlayerUI.isInGame)
            return;
        
        if(!_hasAnimator) return;

        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if(_inputManager.Crouch) targetSpeed = 1.5f;
        if(_inputManager.Move ==Vector2.zero) targetSpeed = 0;

        if(_grounded) 
        { 
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y =  Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0 , zVelDifference)), ForceMode.VelocityChange);
        } else
        {
            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * AirResistance,0,_currentVelocity.y * AirResistance)), ForceMode.VelocityChange);
        }
        
        _animator.SetFloat(_xVelHash , _currentVelocity.x);
        _animator.SetFloat(_yVelHash, _currentVelocity.y);
    }

    private void CamMovements()
    {
        if(PlayerUI.isPaused)
            return;
        
        if(!_hasAnimator) return;

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        Camera.position = CameraRoot.position;
        
        
        _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void HandleCrouch() => _animator.SetBool(_crouchHash , _inputManager.Crouch);


    private void HandleJump()
    {
        if(PlayerUI.isPaused) return;
        if(!_hasAnimator) return;
        if(!_inputManager.Jump) return;
        if(!_grounded) return;
        _animator.SetTrigger(_jumpHash);

        //Enable this if you want B-Hop
        //_playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
        //_playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
        //_animator.ResetTrigger(_jumpHash);
    }

    public void JumpAddForce()
    {
        //Comment this out if you want B-Hop, otherwise the player will jump twice in the air
        _playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
        _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
        _animator.ResetTrigger(_jumpHash);
    }

    private void SampleGround()
    {
        if(!_hasAnimator) return;
        
        RaycastHit hitInfo;
        if(Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, Dis2Ground + 0.1f, GroundCheck))
        {
            //Grounded
            _grounded = true;
            SetAnimationGrounding();
            return;
        }
        //Falling
        _grounded = false;
        _animator.SetFloat(_zVelHash, _playerRigidbody.velocity.y);
        SetAnimationGrounding();
        return;
    }
    
    private void HandleAiming()
    {
        if(PlayerUI.isPaused)
            return;
        
        bool isAiming = _inputManager.Aiming;

        if (isAiming)
        {
            _animator.SetBool(_aimingHash, true);
            AimRigWeight = 1f;
        }
        else
        {
            _animator.SetBool(_aimingHash, false);
            AimRigWeight = 0f;
        }
        
        AimRig.weight = Mathf.Lerp(AimRig.weight, AimRigWeight, 20f * Time.deltaTime);
    }

    private void SetAnimationGrounding()
    {
        _animator.SetBool(_fallingHash, !_grounded);
        _animator.SetBool(_groundHash, _grounded);
    }
    
    // Method called from the Animation Events files of the player
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if(PlayerUI.isPaused)
            return;
        
        if (FootstepAudioClips.Length > 0)
        {
            _player.FootStepAudioSound();
        }
    }

    public void PlayIOSMessageSound()
    {
        if (isLocalPlayer)
        {
            AudioSource.PlayClipAtPoint(IOSNotificationMessageClip, transform.TransformPoint(_playerRigidbody.transform.position), 1f);
        }
    }

    // Method called from the Animation Events files of the player
    private void OnLand(AnimationEvent animationEvent)
    {
        if(isLocalPlayer)
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_playerRigidbody.transform.position), 1f);
    }
}
