using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SyncCamSize : MonoBehaviour
{
    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
        var test = Camera.main.GetComponent<PixelPerfectCamera>();
        _cam.orthographicSize = test.CorrectCinemachineOrthoSize(13);
    }
}
