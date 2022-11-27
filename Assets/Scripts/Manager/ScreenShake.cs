using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoSingleton<ScreenShake>
{

    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        { 
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                CinemachineBasicMultiChannelPerlin camPerlin = GameManager.Instance.SceneVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeScreen(float intensity, float duration)
    {
        CinemachineBasicMultiChannelPerlin camPerlin = GameManager.Instance.SceneVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camPerlin.m_AmplitudeGain = intensity;
        _timer = duration;
    }
}
