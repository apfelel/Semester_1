using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SyncCamSize : MonoBehaviour
{
    private Camera _cam;
    CinemachineVirtualCamera _vCam;

    [SerializeField]
    float _sizeAdd;
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam != null)
        {
            var test = Camera.main.GetComponent<PixelPerfectCamera>();
            _cam.orthographicSize = test.CorrectCinemachineOrthoSize(LVLManager.Instance.CamSize) + _sizeAdd;
        }
        else
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
            var test = Camera.main.GetComponent<PixelPerfectCamera>();
            _vCam.m_Lens.OrthographicSize = test.CorrectCinemachineOrthoSize(LVLManager.Instance.CamSize) + _sizeAdd;
        }
    }
}
