using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DashReset : MonoBehaviour
{
    [SerializeField]
    private GameObject _particle;

    [SerializeField]
    private Sprite _activeSprite, _inactiveSprite;
    [SerializeField]
    private float _delay;
    private SpriteRenderer _sr;

    private Light2D _light;

    bool _isActive = true;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !GameManager.Instance.PlayerVar.HasDash)
        {
            if (_isActive)
            {
                var gb = Instantiate(_particle);
                gb.transform.position = transform.position;
                StartCoroutine(DisabledForXSec(_delay));
            }
        }
    }

    private IEnumerator DisabledForXSec(float time)
    {
        _light.enabled = false;
        _isActive = false;
        _sr.sprite = _inactiveSprite;
        yield return new WaitForSeconds(time);
        _sr.sprite = _activeSprite;
        _isActive = true;
        _light.enabled = true;
    }
}
