using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightManager : MonoSingleton<GlobalLightManager>
{
    [SerializeField]
    private Light2D _front, _mid, _back;

    public bool State;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (State)
        {
            _front.intensity = Mathf.Lerp(_front.intensity, 0.8f, 0.05f);
            _mid.intensity = Mathf.Lerp(_mid.intensity, 0.8f, 0.05f);
            _back.intensity = Mathf.Lerp(_back.intensity, 0.8f, 0.05f);
        }
        else
        {
            _front.intensity = Mathf.Lerp(_front.intensity, 0.9f, 0.05f);
            _mid.intensity = Mathf.Lerp(_mid.intensity, 0.3f, 0.05f);
            _back.intensity = Mathf.Lerp(_back.intensity, 0.25f, 0.05f);
        }
    }


}
