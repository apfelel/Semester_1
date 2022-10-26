using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallClimb : MonoBehaviour
{
    private bool _wallSlide;
    private Rigidbody2D _rb;

    [SerializeField]
    private int _jumpForceMult;
    [SerializeField]
    private Vector2 _jumpForceStrength;

    [Range(0f, 1f)]
    [SerializeField]
    private float _yDampMult;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_wallSlide)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * (1- _yDampMult));
            }
        }
    }
    public void StartWallSlide()
    {
        _wallSlide = true;
    }

    public void EndWallSlide()
    {
        _wallSlide= false;
    }

    public void JumpOff(Vector2 wallHit)
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = new Vector2(0, _rb.velocity.y / 5);
        else
            _rb.velocity = new Vector2(0, _rb.velocity.y);

        int dir;

        if (wallHit.x - transform.position.x > 0)
            dir = 1;
        else
            dir = -1;

        _rb.velocity = 
            transform.up * Mathf.Max(_jumpForceStrength.y * _jumpForceMult, _rb.velocity.y) +
            Vector3.right * -_jumpForceStrength.x * dir * _jumpForceMult;
        _wallSlide = false;
    }
}
