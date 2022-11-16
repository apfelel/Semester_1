using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private AudioClip _crystalPickup;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();   
    }
    public void CrystalPickup()
    {
        _source.PlayOneShot(_crystalPickup);
    }
}
