using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(
    typeof(PlayerInputActions))]

[RequireComponent(
    typeof(PlayerMovement),
    typeof(Grapple),
    typeof(PlayerDash))]

[RequireComponent(
    typeof(PlayerWallClimb))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Grapple _grapple;
    private PlayerDash _playerDash;
    private PlayerWallClimb _wallClimb;

    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _grappleAction;
    private InputAction _dashAction;
    private InputAction _verticalAction;
    private InputAction _pauseAction;

    [HideInInspector]
    public Rigidbody2D Rb;
    
    private float _moveInput;
    private PlayerVar _playerVar;
    private void OnEnable()
    {
        EnableInput();
        _pauseAction = _playerInputActions.Player.Pause;
        _pauseAction.performed += Pause;
        _pauseAction.Enable();
    }

    private void EnableInput()
    {
        _moveAction = _playerInputActions.Player.Move;
        _moveAction.Enable();

        _jumpAction = _playerInputActions.Player.Jump;
        _jumpAction.canceled += JumpReleased;
        _jumpAction.performed += Jump;
        _jumpAction.Enable();

        _grappleAction = _playerInputActions.Player.Grapple;
        _grappleAction.performed += StartGrapple;
        _grappleAction.canceled += EndGrapple;
        _grappleAction.Enable();

        _dashAction = _playerInputActions.Player.Dash;
        _dashAction.performed += Dash;
        _dashAction.Enable();

        _verticalAction = _playerInputActions.Player.Vertical;
        _verticalAction.Enable();
    }

    private void OnDisable()
    {
        DisableInput();
        _pauseAction.Disable();
    }
    private void DisableInput()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _grappleAction.Disable();
        _dashAction.Disable();
        _verticalAction.Disable();
    }

    void Awake()
    {
        
        _playerDash = GetComponent<PlayerDash>();
        Rb = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerMovement = GetComponent<PlayerMovement>();
        _grapple = GetComponent<Grapple>();
        _wallClimb = GetComponent<PlayerWallClimb>();
        _playerVar = GetComponent<PlayerVar>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += ControlsChanged;
        GameManager.Instance.CurControlScheme = _playerInput.currentControlScheme;
    }

    private void ControlsChanged(PlayerInput obj)
    {
        GameManager.Instance.CurControlScheme = obj.currentControlScheme;
    }

    void FixedUpdate()
    {
        _moveInput = _moveAction.ReadValue<float>();
        _playerMovement.Move(_moveInput, _playerVar.IsGrounded, _playerVar.IsHooked, _playerVar.IsWeakened? 0.5f: 1);
        

        if (_playerVar.HasGloves && !_playerVar.IsHooked && _playerVar.IsWallInFront && !_playerVar.IsGroundedBuffered && _moveInput != 0)
        {
            _playerVar.IsWallsliding = true;
            _wallClimb.StartWallSlide();
        }
        else
        {
            _playerVar.IsWallsliding = false;
            _wallClimb.EndWallSlide();
        }

        _playerVar.JumpBuffer -= Time.deltaTime;
        if (_playerVar.JumpBuffer > 0 && !_playerVar.IsHooked && _playerVar.IsGroundedBuffered & !_playerVar.IsWallsliding & !_playerVar.Jumped)
        {
            if (!_jumpAction.IsPressed())
            {
                _playerMovement.Jump(true);
            }
            else
            {
                _playerMovement.Jump(false);
                _playerVar.Jumped = true;
            }
            _playerVar.JumpBuffer = -1;
        }
        if (_playerVar.JumpBuffer > 0 && !_playerVar.IsHooked & !_playerVar.IsGrounded && _playerVar.IsWallSlidingBuffered)
        {
            _playerVar.JumpBuffer = -1;
            _wallClimb.JumpOff(_playerVar.LastWallHitPos);
        }
    }

    public void SkipGroundedBuffer()
    {
        Debug.Log(Rb.velocity.y);
        _playerVar.LastGrounded = 500;
        _playerVar.GroundDeny = 0.5f;
    }
    public void Dash(InputAction.CallbackContext obj)
    {
        if (!_playerVar.IsHooked)
        {
            _playerDash.Dash(new Vector2(_moveInput, _verticalAction.ReadValue<float>()).normalized);
        }
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        _playerVar.JumpBuffer = _playerVar.CoyoteTime;
        _grapple.StartDrawingIn();
    }
    private void JumpReleased(InputAction.CallbackContext obj)
    {
        _grapple.StopDrawingIn();

        if (_playerVar.Jumped)
        {
            _playerVar.Jumped = false;
            _playerMovement.ShortenJump();
        }
    }
    private void StartGrapple(InputAction.CallbackContext obj)
    {
        if(_playerVar.HasGrapple)
         _grapple.StartGrapple();
    }
    private void EndGrapple(InputAction.CallbackContext obj)
    {
        _grapple.EndGrapple();
    }
    private void Pause(InputAction.CallbackContext obj)
    {
       
        UIManager.Instance.SwitchPause();
        if (UIManager.Instance.IsPaused)
            EnableInput();
        else
            DisableInput();
    }

    public void WeakenedState(bool isActive)
    {
        DisableInput();
        if (isActive)
        {
            _moveAction = _playerInputActions.Player.Move;
            _moveAction.Enable();
        }
        else
        {
            EnableInput();
        }
        _playerVar.IsWeakened = isActive;
        Debug.Log("weak");
    }
}
