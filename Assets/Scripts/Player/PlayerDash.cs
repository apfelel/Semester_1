using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _ps;

    [SerializeField]
    private float _delay, _strength;
    private bool _dashed, _hasDash;

    private PlayerController _playerController;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_hasDash &! _dashed)
            if(_playerController.IsGrounded)
                _hasDash = true;
    }

    public void Dash(Vector2 dir)
    {

        if (_hasDash && !_dashed &! _playerController.IsHooked)
        {

            _rb.velocity = Vector2.zero;
            _rb.AddForce(dir * _strength);
            _ps.gameObject.transform.right = -dir;
            _ps.Play();
            StartCoroutine(DashDelay());
        }
    }

    private IEnumerator DashDelay()
    {
        _hasDash = false;
        _dashed = true;
        yield return new WaitForSeconds(_delay);
        _dashed = false;
    }
}
