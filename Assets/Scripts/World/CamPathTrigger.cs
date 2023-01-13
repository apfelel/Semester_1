using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPathTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _vCam;
    [SerializeField]
    private bool _resetOnMove = true;

    [SerializeField]
    private CinemachineSmoothPath _smoothPath;

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
            if(_resetOnMove)
                if (_playerRB.velocity.magnitude > 0.1f)
                {
                    _vCam.Priority = -20;
                    _timer = 0;
                }


            if (_timer < _delayToSwitch)
            {
                _timer += Time.deltaTime;

                if (_timer > _delayToSwitch / 2)
                {
                    _smoothPath.m_Waypoints[0].position = Camera.main.gameObject.transform.position - _smoothPath.transform.position;
                    _vCamTrack.m_PathPosition = -0.2f;
                }
            }
            if (_timer > _delayToSwitch)
            {
                _vCam.Priority = 20;
                _vCamTrack.m_PathPosition += Time.deltaTime / 2;
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
