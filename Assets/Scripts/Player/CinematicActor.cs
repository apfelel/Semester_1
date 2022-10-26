using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicActor : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;
    private bool _inControl;
    private float _dir;
    private float _duration;
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (!_inControl) return;
        _duration -= Time.deltaTime;
        _playerMovement.Move(_dir, true, false);
        if (_duration < 0)
        {
            _playerController.enabled = true;
            _inControl = false;
        }
    }
    public void MoveXSec(float dir, float duration)
    {
        _inControl = true;
        _dir = dir;
        _duration = duration;
        _playerController.enabled = false;
    }
}
