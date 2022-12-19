using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField]
    private AudioMixer _audioMixer;
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
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private AudioSource _ambientSource;

    public enum AudioNames
    {
        SFX, Music, Master, Ambient
    }
    private void Start()
    {
        _musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        _ambientSource = transform.GetChild(1).GetComponent<AudioSource>();

        var _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", -1);
        if (_sfxVolume == -1)
        {
            _sfxVolume = 1;
            PlayerPrefs.SetFloat("SfxVolume", 1);
        }
        ChangeVolume(AudioNames.SFX, _sfxVolume);

        var _masterVolume = PlayerPrefs.GetFloat("MasterVolume", -1);
        if (_masterVolume == -1)
        {
            _masterVolume = 1;
            PlayerPrefs.SetFloat("MasterVolume", 1);
        }
        ChangeVolume(AudioNames.SFX, _masterVolume);

        var _ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", -1);
        if (_ambientVolume == -1)
        {
            _ambientVolume = 1;
            PlayerPrefs.SetFloat("AmbientVolume", 1);
        }
        ChangeVolume(AudioNames.SFX, _ambientVolume);

        var _musicVolume = PlayerPrefs.GetFloat("MusicVolume", -1);
        if (_musicVolume == -1)
        {
            _musicVolume = 1;
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        ChangeVolume(AudioNames.Music, _musicVolume);
       
        
        PlayerPrefs.Save();
        _sfxSource = GetComponent<AudioSource>();   
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
        _sfxSource.PlayOneShot(clips?[Random.Range(0, clips.Count - 1)], volume);
    }

    public void PlayMusic(string name)
    {
        _musicSource.clip = _musicDic.GetValueOrDefault(name);
        _musicSource.Play();
    }
    public void ChangeVolume(AudioNames name, float value)
    {
        _audioMixer.SetFloat(name.ToString(), Mathf.Log(value) * 20f);
    }
}
