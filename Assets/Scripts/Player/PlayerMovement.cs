using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    [TabGroup("Speed")]
    [SerializeField]
    private float _speedMod;
    [TabGroup("Damp")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _groundedDamp; 
    [TabGroup("Damp")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _airDamp;
    [TabGroup("Damp")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _hookDamp;
    [TabGroup("Jump")]
    [SerializeField]
    private float _jumpForce;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Move(float dir, bool isGrounded, bool isHooked)
    {
        var signedDir = 0;
        if (dir != 0)
            signedDir = MathF.Sign(dir);
        if (_rb.velocity.x < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (_rb.velocity.x > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        float damp;
        int hasSpeedLimit= 1;

        if (isGrounded)
        {
            damp = 0;
        }
        else if (isHooked)
        {
            damp = _hookDamp;
            //hasSpeedLimit = 0;
        }
        else
            damp = _airDamp;

        damp = 1 - damp;
        float curSpeed = _rb.velocity.x;

        curSpeed += signedDir * _speedMod * damp;

        curSpeed *= 1 - (_groundedDamp * damp) * hasSpeedLimit;

        _rb.velocity = new Vector2(curSpeed, _rb.velocity.y);
    }
    public void Jump(bool shortenedJump = false)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        if (shortenedJump)
            _rb.AddForce(Vector2.up * _jumpForce * 0.6f);
        else
            _rb.AddForce(Vector2.up * _jumpForce);
    }
    public void ShortenJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.6f);
    }

}
