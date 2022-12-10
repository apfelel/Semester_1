using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleton<SoundManager>
{
    [HideInInspector]
    public float SfxVolume;
    private float musicVolume;
    public float MusicVolume
    {
        get => musicVolume; 

        set
        {
            musicVolume = value;
            RefreshSoundVolume();
        }
    }

    [System.Serializable]
    private struct Sound
    {
        public string Name;
        public List<AudioClip> Clips;
    }
    [System.Serializable]
    private struct Music
    {
        public string Name;
        public AudioClip Clip;
    }
    [SerializeField]
    private List<Sound> _soundsList = new();
    [SerializeField]
    private List<Music> _musicList = new();
    private Dictionary<string, List<AudioClip>> _soundsDic = new();
    private Dictionary<string, AudioClip> _musicDic = new();
    private AudioSource _source;
    private AudioSource _sourceMusic;

   
    private void Start()
    {
        _sourceMusic = transform.GetChild(0).GetComponent<AudioSource>();

        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", -1);
        if (SfxVolume == -1)
        {
            SfxVolume = 1;
            PlayerPrefs.SetFloat("SfxVolume", 1);
            PlayerPrefs.Save();
        }

        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", -1);
        if (MusicVolume == -1)
        {
            MusicVolume = 1;
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.Save();
        }

        _source = GetComponent<AudioSource>();   
        DontDestroyOnLoad(this);
        _soundsList.ForEach((s) => _soundsDic.Add(s.Name, s.Clips));
        _musicList.ForEach((s) => _musicDic.Add(s.Name, s.Clip));

        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        PlayMusic(LVLManager.Instance.MusicName);
    }

    public void PlaySound(string name, float volume)
    {
        var clips = _soundsDic[name];
        _source.PlayOneShot(clips?[Random.Range(0, clips.Count - 1)], volume * SfxVolume);
    }

    public void PlayMusic(string name)
    {
        _sourceMusic.clip = _musicDic.GetValueOrDefault(name);
        _sourceMusic.Play();
    }
    private void RefreshSoundVolume()
    {
        _sourceMusic.volume = musicVolume / 5;
    }

}
