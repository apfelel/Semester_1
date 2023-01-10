using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private GameObject _dashPS;
    private Animator _anim;
    private Rigidbody2D _rb;
    private PlayerVar _playerVar;
    private Grapple _grapple;

    [SerializeField]
    private SpriteRenderer _gfx;

    [SerializeField]
    private ParticleSystem _hair;
    [SerializeField]
    private ParticleSystem _dashRefresh;

    private List<ParticleSystem.MainModule> _ps;
    private bool _dashAnimPlayed;
    [SerializeField]
    private Gradient _dodgedHairColor;
    private Gradient _startHairColor;
    [SerializeField]
    private Gradient _curHairColor;

    [SerializeField]
    private GameObject _rotationObjectX;
    private GameObject _rotationObject;

    private float _lastRot;
    private void Start()
    {
        _rotationObject = _rotationObjectX.transform.GetChild(0).gameObject;
        _playerVar = GetComponent<PlayerVar>();
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _grapple = GetComponent<Grapple>();

        _ps = new List<ParticleSystem.MainModule>(GetComponentsInChildren<ParticleSystem>().Select(p => p.main));
        var colOverT = _hair.colorOverLifetime;
        _startHairColor = colOverT.color.gradient;
        _curHairColor = colOverT.color.gradient;
    }

    private void LateUpdate()
    {
        
        var colOverT = _hair.colorOverLifetime;
        if (_playerVar.HasDash)
        {
            var keys = _curHairColor.colorKeys;
            for (int i = 0; i < _startHairColor.colorKeys.Count(); i++)
            {
                keys[i].color = _curHairColor.colorKeys[i].color * 0.95f + _startHairColor.colorKeys[i].color * 0.05f;
            }
            _curHairColor.SetKeys(keys, _curHairColor.alphaKeys);
        }
        else
        {
            var keys = _curHairColor.colorKeys;
            for (int i = 0; i < _startHairColor.colorKeys.Count(); i++)
            {
                keys[i].color = _curHairColor.colorKeys[i].color * 0.95f + _dodgedHairColor.colorKeys[i].color * 0.05f;
            }
            _curHairColor.SetKeys(keys, _curHairColor.alphaKeys);
        }
        colOverT.color = _curHairColor;
        _ps.ForEach((p) => p.emitterVelocity = _rb.velocity);

        if (Mathf.Abs(_rb.velocity.x) < 1.4f)
        {
            _anim.SetBool("Walking", false);
        }
        else
            _anim.SetBool("Walking", true);

        _anim.SetBool("Weakened", _playerVar.IsWeakened);
        _anim.SetFloat("AbsVelX", Mathf.Abs(_rb.velocity.x));
        _anim.SetBool("Grounded", _playerVar.IsGrounded);
        _anim.SetFloat("Vel", _rb.velocity.magnitude);

        if (!_playerVar.IsDashing)
            _dashAnimPlayed = false;
        else if(!_dashAnimPlayed)
        {
            _dashAnimPlayed = true;
            if (_rb.velocity.y < 1 && _rb.velocity.y > -1)
            {
                _dashPS.transform.localRotation = Quaternion.Euler(0, 0, 180);
                _anim.Play("Dash_Side");
            }
            else
            {
                if (_rb.velocity.y > 1)
                {
                    if(Mathf.Abs(_rb.velocity.x) > 0.1f)
                        _dashPS.transform.localRotation = Quaternion.Euler(0, 0, -135);
                    else
                        _dashPS.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    _anim.Play("Dash_Up");
                }
                else
                {
                    if (Mathf.Abs(_rb.velocity.x) > 0.1f)
                        _dashPS.transform.localRotation = Quaternion.Euler(0, 0, 135);
                    else
                        _dashPS.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    _anim.Play("Dash_Down");
                }
            }
        }

        

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

        if (!_playerVar.IsHooked)
        {
            if (_rb.velocity.x < -0.1f)
            {
                _gfx.flipX = true;
                _rotationObjectX.transform.rotation = Quaternion.Euler(0, 180, 0);
                //_rotationObject.transform.localRotation = _gfx.transform.rotation;
            }
            if (_rb.velocity.x > 0.1f)
            {
                _gfx.flipX = false;
                _rotationObjectX.transform.rotation = Quaternion.Euler(0, 0, 0);
                //_rotationObject.transform.localRotation = _gfx.transform.rotation;
            }
        }
        else
            _rotationObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (_playerVar.IsHooked)
        {
            _gfx.transform.up = Vector2.Lerp(_gfx.transform.up, _grapple.CurAnchor - _gfx.transform.position, Time.deltaTime * 4);

            var curRot = _gfx.transform.rotation.eulerAngles.z;
            curRot %= 360;
            if (curRot < 0) curRot += 360;

            if (Mathf.Abs(curRot - _lastRot) < 50)
            {
                if (curRot < _lastRot)
                {
                    _rotationObjectX.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    _gfx.flipX = true;
                }
                else
                {
                    _rotationObjectX.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    _gfx.flipX = false;
                }
            }

            _lastRot = curRot;
        }
        else 
        {
            _gfx.transform.up = Vector2.Lerp(_gfx.transform.up, Vector2.up, Time.deltaTime * 6);
            _rotationObjectX.transform.localRotation = Quaternion.Euler(0, _rotationObjectX.transform.localRotation.eulerAngles.y, 0);
        }
    }
    public void DeathAnim()
    {
        _anim.Play("Death");
    }
    public void Idle()
    {
        _anim.Play("Idle");
    }
    public void RespawnAnim()
    {
        _anim.Play("Spawn");
    }

    public void RefreshDash()
    {
        _dashRefresh.Play();
    }
}
