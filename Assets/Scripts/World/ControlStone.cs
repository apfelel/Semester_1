using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlStone : MonoBehaviour
{
    [SerializeField]
    private Sprite _inactive, _activeKey, _activeContr;

    private SpriteRenderer _sr;
    // Update is called once per frame
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.CurControlScheme == "Keyboard + Mouse")
            {
                _sr.sprite = _activeKey;
            }
            else
                _sr.sprite = _activeContr;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _sr.sprite = _inactive;
    }
}
