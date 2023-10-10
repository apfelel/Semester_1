using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _sfxSlider;
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _ambientSlider;

    [Space]
    [SerializeField]
    private Animator _animFade;
    [SerializeField]
    private GameObject _pauseMenue;
    [SerializeField]
    private GameObject _settingMenue;
    [SerializeField]
    private RawImage _pixelatedImage;

    [Space]
    [SerializeField]
    private TextMeshProUGUI _gemCountTxt;
    [SerializeField]
    private TextMeshProUGUI _gemDelayedCountTxt;
    [SerializeField]
    private RectTransform _gemParent;
    [SerializeField]
    private RectTransform _gemShownPos;
    private Vector2 _gemHiddenPos;
    private int _shownGemCount;
    private int _trueGemCount;
    private float _showTimer = 100;
    [HideInInspector]
    public bool IsPaused;
    [HideInInspector]
    public bool InSetting;
    private void Start()
    {
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
        _gemHiddenPos = _gemParent.anchoredPosition;
        DontDestroyOnLoad(gameObject);
    }
    private void FixedUpdate()
    {
        _showTimer += Time.deltaTime;
        if(_showTimer > 2)
        {
            _gemParent.anchoredPosition = Vector2.Lerp(_gemParent.anchoredPosition, _gemHiddenPos, 0.05f);
        }
        else
        {
            _gemParent.anchoredPosition = Vector2.Lerp(_gemParent.anchoredPosition, _gemShownPos.anchoredPosition, 0.2f);
        }

        if(_showTimer > 1)
        {
            if(_shownGemCount < _trueGemCount)
            {
                _showTimer = 1;
                _shownGemCount++;
                _gemCountTxt.text = _shownGemCount.ToString();
                _gemDelayedCountTxt.text = (_trueGemCount - _shownGemCount).ToString();
            }
        }
    }
    public void Pause()
    {
        IsPaused = true;
        _pauseMenue.SetActive(true);
        _settingMenue.SetActive(false);
        _pauseMenue.GetComponentInChildren<Selectable>().Select();
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        InSetting = false;
        IsPaused = false;
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
        GameManager.Instance.PlayerController.EnableInput();
        Time.timeScale = 1f;
    }

    public void ResetGemCount(int amount = 0)
    {
        _trueGemCount = amount;
    }
    public void SwitchPause()
    {
        if (IsPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void OpenSettings()
    {
        if(GameManager.Instance.IsActive)
            _pauseMenue.SetActive(false);
        _settingMenue.SetActive(true);
        _settingMenue.GetComponentInChildren<Selectable>().Select();

        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _ambientSlider.value = PlayerPrefs.GetFloat("AmbientVolume");

        InSetting = true;
    }

    public void CloseSettings()
    {
        if (GameManager.Instance.IsActive)
        {
            _pauseMenue.SetActive(true);
            _pauseMenue.GetComponentInChildren<Selectable>().Select();
        }
        else
            EventSystem.current.SetSelectedGameObject(null);

        PlayerPrefs.Save();
        _settingMenue.SetActive(false);
        InSetting = false;
    }
    public void AllignPixelImage(Vector2 camPos, float curScreenSize, float _renderTextureHeight)
    {
        float sizeConst = _renderTextureHeight / (curScreenSize * 16);
        Debug.Log(sizeConst);
        //_pixelatedImage.rectTransform.anchoredPosition = new Vector2(camPos.x % sizeConst, camPos.y % sizeConst);
    }
    public void UpdateGemCount(int num)
    {
        _showTimer = 0;
        _trueGemCount = num;
        _gemDelayedCountTxt.text = (_trueGemCount - _shownGemCount).ToString();
    }
    public void FadeIn()
    {
        _animFade.Play("FadeIn");
    }
    public void FadeOut()
    {
        _animFade.Play("FadeOut");
    }
    public void ChangeMusicVolume(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundManager.AudioNames.Music, value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public void ChangeSfxVolume(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundManager.AudioNames.SFX, value);
        PlayerPrefs.SetFloat("SfxVolume", value);
    }
    public void ChangeMasterVolume(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundManager.AudioNames.Master, value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public void ChangeAmbientVolume(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundManager.AudioNames.Ambient, value);
        PlayerPrefs.SetFloat("AmbientVolume", value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        GameManager.Instance.Deactivate();
        UnPause();
        SceneManager.LoadScene("MainMenu");
    }
}