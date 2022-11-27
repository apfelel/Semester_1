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
    private void RecalculateCamSize(float camSize)
    {
        if (_cam != null)
        {
            var test = Camera.main.GetComponent<PixelPerfectCamera>();
            _cam.orthographicSize = test.CorrectCinemachineOrthoSize(camSize);
        }
        else
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
            var test = Camera.main.GetComponent<PixelPerfectCamera>();
            _vCam.m_Lens.OrthographicSize = test.CorrectCinemachineOrthoSize(camSize);
        }

        Debug.Log("test");
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScreensizeChange -= RecalculateCamSize;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnScreensizeChange += RecalculateCamSize;
        _cam = GetComponent<Camera>();
    }
}
