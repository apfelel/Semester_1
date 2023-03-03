using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LVLManager :MonoSingleton<LVLManager>
{
    public float CamSize;
    public float VelocityLimit;

    public string MusicName;

    public string AmbientName;

    public GameManager.Direction DefaultExit;

    public bool HasGloves, HasGrapple;
}
