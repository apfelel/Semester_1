using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationActions : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _flakesBuildUp;
    [SerializeField]
    private ParticleSystem _flakesBuildDown;
    [SerializeField]
    private ParticleSystem _dash;
    [SerializeField]
    private ParticleSystem _flakes;
    public void PlayFlakesBuildUp(float duration)
    {
        _flakesBuildUp.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        var ps = _flakesBuildUp.main;
        ps.duration = duration;
        _flakesBuildUp.Play();
    }
    
    public void PlayFlakesBuildDown(float duration)
    {
        _flakesBuildDown.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        var ps = _flakesBuildDown.main;
        ps.duration = duration;
        _flakesBuildDown.Play();
    }

    public void PlayDash()
    {
        _dash.Play();
    }
    
    public void PlayFlakes(float duration)
    {
        _flakes.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        var ps = _flakes.main;
        ps.duration = duration;
        _flakes.Play();
    }

    public void StopAll()
    {
        _flakes.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        _flakesBuildDown.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        _flakesBuildUp.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
