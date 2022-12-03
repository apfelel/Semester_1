using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [System.Serializable]
    private struct Sound
    {
        public string Name;
        public List<AudioClip> Clips;
    }
    [SerializeField]
    private List<Sound> _soundsList = new();

    private Dictionary<string, List<AudioClip>> _soundsDic = new();

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();   
        DontDestroyOnLoad(this);

        _soundsList.ForEach((s) => _soundsDic.Add(s.Name, s.Clips));
    }
    public void PlaySound(string name)
    {
        var clips = _soundsDic[name];
        _source.PlayOneShot(clips?[Random.Range(0, clips.Count - 1)]);
    }
}
