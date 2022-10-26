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

    private PlayerInputActions _playerInputActions;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _grappleAction;
    private InputAction _dashAction;
    private InputAction _verticalAction;

    public Rigidbody2D Rb;
    [Header("Hook Things")]
    [HideInInspector]
    public bool IsHooked;
    [HideInInspector]
    public List<GameObject> AnchorsInScene = new List<GameObject>();

    [Header("Grounded Things")]
    [SerializeField]
    private float _coyoteTime;
    [SerializeField]
    private Transform _groundedRayPos;

    [SerializeField]
    private LayerMask _groundLayer;

    private  float _lastGrounded;
    [HideInInspector]
    public bool IsGroundedBuffered
    {
        get
        {
            if (_lastGrounded < _coyoteTime)
                return true;
            return false;
        }
    }
    private bool _isGrounded;
    [HideInInspector]
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            if (value == true)
                _lastGrounded = 0;
            _isGrounded = value;        
        }
    }
    private float _groundDeny;

    [Header("Jump Things")]

    private bool _jumped;
    private float _moveInput;
    [HideInInspector]
    public bool Jumped
    {
        get => _jumped;
        set
        {
            _jumped = value;
            _lastGrounded = 500;
        }
    }
    private float _jumpBuffer;

    [Header("Wall Things")]
    private bool _isWallsliding;
    public bool IsWallsliding
    {
        get
        {
            return _isWallsliding;
        }
        set
        {
            if (value == true)
                _lastWallSlide = 0;
            _isWallsliding = value;
        }
    }
    [SerializeField]
    private Transform _wallRayPos;
    private float _lastWallSlide;
    [HideInInspector]
    public bool IsWallSlidingBuffered
    {
        get
        {
            if (_lastWallSlide < _coyoteTime)
                return true;
            return false;
        }
    }
    private bool _isWallInFront;
    private Vector2 _lastWallHitPos;

    private void OnEnable()
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
        AnchorsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor"));
    }

    void FixedUpdate()
    {
        _groundDeny -= Time.deltaTime;
        _lastGrounded += Time.deltaTime;
        _lastWallSlide += Time.deltaTime;
        IsGrounded = false;
        _isWallInFront = false;

        if (Physics2D.Raycast(_groundedRayPos.position, Vector2.down, 0.1f, _groundLayer) &! Jumped && Rb.velocity.y < 0.01f && _groundDeny < 0)
            IsGrounded = true;
        RaycastHit2D hit = Physics2D.Raycast(_wallRayPos.position, transform.right, 0.3f, _groundLayer);
        if (hit == true)
        {
            _isWallInFront = true;
            _lastWallHitPos = hit.point;
        }
        if (Jumped && Rb.velocity.y < 0)
            Jumped = false;

        _moveInput = _moveAction.ReadValue<float>();
        _playerMovement.Move(_moveInput, IsGrounded, IsHooked);
        

        if (!IsHooked && _isWallInFront && !IsGroundedBuffered && _moveInput != 0)
        {
            IsWallsliding = true;
            _wallClimb.StartWallSlide();
        }
        else
        {
            IsWallsliding = false;
            _wallClimb.EndWallSlide();
        }

        _jumpBuffer -= Time.deltaTime;
        if (_jumpBuffer > 0 && !IsHooked && IsGroundedBuffered & !IsWallsliding & !Jumped)
        {
            if (!_jumpAction.IsPressed())
            {
                _playerMovement.Jump(true);
            }
            else
            {
                _playerMovement.Jump(false);
                Jumped = true;
            }
            _jumpBuffer = -1;
        }
        if (_jumpBuffer > 0 && !IsHooked & !IsGrounded && IsWallSlidingBuffered)
        {
            _jumpBuffer = -1;
            _wallClimb.JumpOff(_lastWallHitPos);
        }
    }

    public void SkipGroundedBuffer()
    {
        Debug.Log(Rb.velocity.y);
        _lastGrounded = 500;
        _groundDeny = 0.5f;
    }
    public void Dash(InputAction.CallbackContext obj)
    {
        if(!IsHooked)
            _playerDash.Dash(new Vector2(_moveInput, _verticalAction.ReadValue<float>()).normalized);
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        _jumpBuffer = _coyoteTime;
        _grapple.StartDrawingIn();
    }
    private void JumpReleased(InputAction.CallbackContext obj)
    {
        _grapple.StopDrawingIn();

        if (Jumped)
        {
            Jumped = false;
            _playerMovement.ShortenJump();
        }
    }
    private void StartGrapple(InputAction.CallbackContext obj)
    {
        _grapple.StartGrapple(GetAvailableHook());
        IsHooked = true;
    }
    private void EndGrapple(InputAction.CallbackContext obj)
    {
        _grapple.EndGrapple();
        IsHooked = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_groundedRayPos.position, _groundedRayPos.position - new Vector3(0, 0.1f, 0));
        Gizmos.DrawLine(_wallRayPos.position, _wallRayPos.position + transform.right * 0.3f);
    }
    private GameObject GetAvailableHook()
    {
        float nearestDist = int.MaxValue;
        GameObject nearestGb = null;
        foreach (var anchor in AnchorsInScene)
        {
            var temp = Vector2.Distance(anchor.transform.transform.position, transform.transform.position);
            if (Vector2.Distance(anchor.transform.transform.position, transform.transform.position) < nearestDist)
            {
                nearestDist = temp;
                nearestGb = anchor;
            }
        }
        return nearestGb;
    }

    public void Die()
    {

    }
}
