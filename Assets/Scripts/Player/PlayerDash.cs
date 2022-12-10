using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [SerializeField]
    private float _minSpeed;
    private Vector2 _dashForce, _endSpeed;
    [SerializeField]
    private ParticleSystem _ps;

    [SerializeField]
    private float _duration, _strength, _cooldown;

    float _cooldownTimer;
    private PlayerVar _playerVar;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_playerVar.HasDash & !_playerVar.IsDashing && _cooldown < _cooldownTimer)
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
            _endSpeed = _dashForce;
            _ps.Play();
            StartCoroutine(DashDelay());
        }
    }

    private IEnumerator DashDelay()
    {
        _rb.gravityScale = 0;
        _playerVar.HasDash = false;
        _playerVar.IsDashing = true;
        yield return new WaitForSeconds(_duration);
        _playerVar.IsDashing = false;
        _rb.gravityScale = 2;
        _rb.velocity = _endSpeed;
    }

    public void RegainDash()
    {
        _cooldownTimer = 10;
        _playerVar.HasDash = true;
    }
}
