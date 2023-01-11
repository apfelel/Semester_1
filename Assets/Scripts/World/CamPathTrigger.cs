using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPathTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _vCam;

    private CinemachineTrackedDolly _vCamTrack;
    bool _inTrigger;
    float _timer;
    [SerializeField]
    float _delayToSwitch;

    private Rigidbody2D _playerRB;

    private void Start()
    {
        _vCamTrack = _vCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _playerRB = GameManager.Instance.PlayerController.Rb;
    }

    void Update()
    {
        if(_inTrigger)
        {

            if (_playerRB.velocity.magnitude > 0.1f)
            {
                _vCam.Priority = -20;
                _timer = 0;
            }


            if (_timer < 2)
            {
                _timer += Time.deltaTime;

                if (_timer > 1)
                {
                    _vCamTrack.m_PathPosition = -0.2f;
                }
            }
            if (_timer > 2)
            {
                _vCam.Priority = 20;
                _vCamTrack.m_PathPosition += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _inTrigger = true;
        _timer = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _inTrigger = false;
        _vCam.Priority = -20;
    }
}
