using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicActor : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private float _dir;

    private Action _controlingActions;
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void LateUpdate()
    {
        if (_controlingActions == null)
            _playerController.enabled = true;
        else
            _controlingActions.Invoke();

    }
    public void MoveXSec(float dir, float duration, float speedMod = 1)
    {
        _dir = dir;
        _playerController.enabled = false;
        StartCoroutine(AddAction(() => _playerMovement.Move(_dir, true, false, speedMod), duration));
    }
    public void Wait(float duration)
    {
        _playerController.enabled = false;
        StartCoroutine(AddAction(() => _rb.velocity = Vector2.zero, duration, null));
    }
    public void Freeze(float duration)
    {
        _playerController.enabled = false;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(AddAction(() => _rb.velocity = Vector2.zero, duration, ()=> _rb.bodyType = RigidbodyType2D.Dynamic));
    }

    private IEnumerator AddAction(Action ac, float duration, Action endAction = null)
    {
        _controlingActions += ac;
        yield return new WaitForSeconds(duration);
        _controlingActions -= ac;
        endAction?.Invoke();
    }
}
