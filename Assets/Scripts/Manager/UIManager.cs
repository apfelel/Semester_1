using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _sfxSlider;

    [Space]
    [SerializeField]
    private Animator _animFade;
    [SerializeField]
    private GameObject _pauseMenue;
    [SerializeField]
    private GameObject _settingMenue;
    [SerializeField]
    private GameObject _audioMenue;
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
        _audioMenue.SetActive(false);
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
        _pauseMenue.SetActive(true);
        _settingMenue.SetActive(false);
        _pauseMenue.GetComponentInChildren<Selectable>().Select();
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
        GameManager.Instance.PlayerController.EnableInput();
        Time.timeScale = 1f;
    }

    public void SwitchPause()
    {
        if (_pauseMenue.activeInHierarchy)
        {
            InSetting = false;
            IsPaused = false;
            UnPause();
        }
        else
        {
            IsPaused = true;
            Pause();
        }
    }

    public void OpenSettings()
    {
        if(GameManager.Instance.IsActive)
            _pauseMenue.SetActive(false);
        _settingMenue.SetActive(true);
        _settingMenue.GetComponentInChildren<Selectable>().Select();
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
        _settingMenue.SetActive(false);
        InSetting = false;
    }

    public void OpenSound()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");

        _audioMenue.GetComponentInChildren<Selectable>().Select();

        _settingMenue.SetActive(false);
        _audioMenue.SetActive(true);
    }
    public void CloseSound()
    {
        _settingMenue.GetComponentInChildren<Selectable>().Select();
        _settingMenue.SetActive(true);
        _audioMenue.SetActive(false);

        PlayerPrefs.SetFloat("MusicVolume", SoundManager.Instance.MusicVolume);
        PlayerPrefs.SetFloat("SfxVolume", SoundManager.Instance.SfxVolume);
        PlayerPrefs.Save();
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
        SoundManager.Instance.MusicVolume = value;
    }
    public void ChangeSfxVolume(float value)
    {
        SoundManager.Instance.SfxVolume = value;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}