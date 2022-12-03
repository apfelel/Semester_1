using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private Sprite _onSprite, _offSprite;
    [SerializeField]
    private GameObject _light;

    private bool _active;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (_active) return;
            _active = true;
            _sr.sprite = _onSprite;
            _light.SetActive(true);
            SoundManager.Instance.PlaySound("Torch");
        }
    }
}
