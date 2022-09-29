using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Grapple _grapple;


    private PlayerInputActions _playerInputActions;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _shoot;
    private InputAction _sprint;

    private Rigidbody2D _rb;

    [Header("Hook Things")]
    [HideInInspector]
    public bool IsHooked;
    public List<GameObject> AnchorsInScene = new List<GameObject>();

    [Header("Grounded Things")]
    [SerializeField]
    private float _groundedBuffer;
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
            if (_lastGrounded < _groundedBuffer &! Jumped)
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

    public bool Jumped 
    { 
        get => _jumped; 
        set 
        { 
            _jumped = value;
            _lastGrounded = 500;
        } 
    }

    private bool _jumped;

    private void OnEnable()
    {
        
        _move = _playerInputActions.Player.Move;
        _move.Enable();

        _jump = _playerInputActions.Player.Jump;
        _jump.performed += _playerMovement.Jump;
        _jump.canceled += _playerMovement.ShortenJump;
        _jump.Enable();

        _shoot = _playerInputActions.Player.Fire;
        _shoot.performed += _grapple.StartGrapple;
        _shoot.canceled += _grapple.EndGrapple;
        _shoot.Enable();

        _sprint = _playerInputActions.Player.Sprint;
        _sprint.performed += _playerMovement.StartSprint;
        _sprint.canceled += _playerMovement.EndSprint;
        _sprint.Enable();
    }


    private void OnDisable()
    {
        _sprint.Disable();
        _move.Disable();
        _jump.Disable();
        _shoot.Disable();
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerMovement = GetComponent<PlayerMovement>();
        _grapple = GetComponent<Grapple>();

        AnchorsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor"));
    }

    void Update()
    {
        _lastGrounded += Time.deltaTime;
        IsGrounded = false;
        if (Physics2D.Raycast(_groundedRayPos.position, Vector2.down, 0.1f, _groundLayer) &! Jumped)
            IsGrounded = true;

        Vector2 moveInput = _move.ReadValue<Vector2>();

        _playerMovement.Move(moveInput.x);

        if (IsGroundedBuffered)
        {
            if (_rb.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (_rb.velocity.x > 0 )
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void SkipGroundedBuffer()
    {
        _lastGrounded = 500;
    }
}
