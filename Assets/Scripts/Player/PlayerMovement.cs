using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;
    private Rigidbody2D _rb;

    private float _sprintMod;
    private float _speed;
    [SerializeField]
    private float _speedMod;
    [SerializeField]
    private float _groundedDamp; 
    [SerializeField]
    private float _airDamp;
    [SerializeField]
    private float _hookDamp;
    [SerializeField]
    private float _jumpForce;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.Jumped && _rb.velocity.y < 0)
            _playerController.Jumped = false;
        
    }
    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_speed, _rb.velocity.y);
    }

    public void Move(float dir)
    {
        float damp;
        int canSprint = 0;
        int hasSpeedLimit= 1;

        if (_playerController.IsGrounded)
        {
            canSprint = 1;
            damp = 0;
        }
        else if (_playerController.IsHooked)
        {
            damp = _hookDamp;
            hasSpeedLimit = 0;
        }
        else
            damp = _airDamp;

        damp = 1 - damp;
        float curSpeed = _rb.velocity.x;

        curSpeed += dir * _speedMod * (1 + _sprintMod * canSprint) * damp;

        curSpeed *= 1 - (_groundedDamp * damp) * hasSpeedLimit;
        _speed = curSpeed;
    }
    public void StartSprint(InputAction.CallbackContext obj)
    {
        _sprintMod = 0.5f;
    }
    public void EndSprint(InputAction.CallbackContext obj)
    {
        _sprintMod = 0f;
    }
    public void Jump(InputAction.CallbackContext obj)
    {
        if (_playerController.IsGroundedBuffered)
        {
            _playerController.Jumped = true;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _jumpForce);
        }
    }
    public void ShortenJump(InputAction.CallbackContext obj)
    {
        if (_playerController.Jumped)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.6f);
            _playerController.Jumped = false;
        }
    }

}
