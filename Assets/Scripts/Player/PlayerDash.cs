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
    private float _duration, _strength;
    private bool _hasDash;

    [HideInInspector]
    public bool Dashing;

    private PlayerController _playerController;
    private PlayerVar _playerVar;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_hasDash &!Dashing)
            if(_playerVar.IsGrounded)
                _hasDash = true;
        if(Dashing)
        {
            _rb.velocity = _dashForce;
        }
    }

    public void Dash(Vector2 dir)
    {

        if (_hasDash && !Dashing & !_playerVar.IsHooked)
        {
            var dot = Vector2.Dot(dir, _rb.velocity.normalized) + _strength - 1;

            //if (Mathf.Abs((dot * _rb.velocity).magnitude) < _minSpeed)
            {
                _dashForce = dir * _minSpeed;
                _endSpeed = _dashForce;
            }
            // else
            {
            //     _dashForce = Mathf.Abs(_rb.velocity.magnitude) * dot * dir;
            //     _endSpeed = _dashForce / ((_strength - 1) / 2 + 1) ;
            }
            Debug.Log(dot);
            Debug.Log(_dashForce);

            _ps.Play();
            StartCoroutine(DashDelay());
        }
    }

    private IEnumerator DashDelay()
    {
        _rb.gravityScale = 0;
        _hasDash = false;
        Dashing = true;
        yield return new WaitForSeconds(_duration);
        Dashing = false;
        _rb.gravityScale = 2;
        _rb.velocity = _endSpeed;
    }

    public void RegainDash()
    {
        _hasDash = true;
    }
}
