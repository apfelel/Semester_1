using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [SerializeField]
    private float _minSpeed;
    private Vector2 _dashForce;
    [SerializeField]
    private ParticleSystem _ps;

    [SerializeField]
    private float _dashDuration, _strength, _cooldown;
    private float _dashTimer;
    float _cooldownTimer;
    private PlayerVar _playerVar;
    private Rigidbody2D _rb;

    private bool _dashResetCheck;
    // Start is called before the first frame update
    void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashResetCheck && _dashDuration >= _dashTimer)
        {
        Debug.Log(_rb.velocity.y);
        Debug.Log(_dashResetCheck);
        Debug.Log(_playerVar.IsDashing);
        Debug.Log(_playerVar.IsDashing);
            _dashResetCheck = _playerVar.IsDashing;
            _dashTimer += Time.deltaTime;
            if (_dashDuration < _dashTimer | !_dashResetCheck)
            {
                _playerVar.IsDashing = false;
                _rb.gravityScale = 2;
            }
        }

        if (!_playerVar.HasDash & !_playerVar.IsDashing && _cooldown < _cooldownTimer)
            if(_playerVar.IsGrounded)
                _playerVar.HasDash = true;
        
        if(_playerVar.IsDashing)
        {
            _rb.velocity = _dashForce;
        }

        _cooldownTimer += Time.deltaTime;
    }

    public void Dash(Vector2 dir)
    {
        if (_playerVar.HasDash && !_playerVar.IsDashing & !_playerVar.IsHooked && _cooldown < _cooldownTimer && dir != Vector2.zero)
        {
            //ScreenShake.Instance.ShakeScreen(3, 0.3f);
            _cooldownTimer = 0;
            if (dir.y < -0.1f)
                dir = new Vector2(dir.x * 0.8f, dir.y * 1.3f);
            else if (dir.y > 0.1f)
                dir = new Vector2(dir.x * 0.8f, dir.y * 0.6f);

            _dashForce = dir * _minSpeed;
            _ps.Play();

            _rb.gravityScale = 0;
            _playerVar.HasDash = false;
            _playerVar.IsDashing = true;
            _dashResetCheck = true;
            _dashTimer = 0;
        }
    }
    public void RegainDash()
    {
        _cooldownTimer = 10;
        _playerVar.HasDash = true;
    }
}
