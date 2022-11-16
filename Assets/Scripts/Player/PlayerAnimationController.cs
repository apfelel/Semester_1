using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    private PlayerVar _playerVar;
    private Grapple _grapple;

    [SerializeField]
    private GameObject _rotationObject;
    [SerializeField]
    private GameObject _gfx;

    [SerializeField]
    private GameObject _hair;
    private void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _grapple = GetComponent<Grapple>();
    }

    private void Update()
    {
        _anim.SetBool("Grounded", _playerVar.IsGrounded);
        if(Mathf.Abs(_rb.velocity.x) < 1.4f)
        {
            _anim.SetBool("Walking", false);
        }
        else
            _anim.SetBool("Walking", true);

        _anim.SetFloat("VelY", _rb.velocity.y);

        if(_playerVar.Jumped || !_playerVar.IsGrounded)
        {
            _anim.SetBool("Midair", true);
        }
        else
        {
            _anim.SetBool("Midair", false);
        }

        _anim.SetBool("Wallslide", _playerVar.IsWallsliding && _rb.velocity.y < 0);
        _anim.SetBool("Hooked", _playerVar.IsHooked);


        
        if (_rb.velocity.x < -0.1f)
        {
            _gfx.transform.localScale = new Vector3(-1, 1, 1);
            _rotationObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (_rb.velocity.x > 0.1f)
        {
            _gfx.transform.localScale = new Vector3(1, 1, 1);
            _rotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (_playerVar.IsHooked)
        {
            transform.up = Vector2.Lerp(transform.up, _grapple.CurAnchor.transform.position - transform.position, Time.deltaTime * 6);
        }
        else 
        {
            transform.up = Vector2.Lerp(transform.up, Vector2.up, Time.deltaTime * 10);
        }
    }
    public void DeathAnim()
    {
        _anim.Play("Death");
        _hair.SetActive(false);
    }
    public void RespawnAnim()
    {
        _anim.Play("Spawn");
        _hair.SetActive(true);
    }
}


