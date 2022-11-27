using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private GameObject _pauseMenue;
    [SerializeField]
    private GameObject _settingMenue;
    [SerializeField]
    private RawImage _pixelatedImage;
    [SerializeField]
    private TextMeshProUGUI _gemCountTxt;

    public bool IsPaused;
    private void Start()
    {
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
        DontDestroyOnLoad(this);
    }
    public void Pause()
    {
        _pauseMenue.SetActive(true);
        _settingMenue.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SwitchPause()
    {
        if (_pauseMenue.activeInHierarchy)
        {
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
    }

    public void CloseSettings()
    {
        if(GameManager.Instance.IsActive)
            _pauseMenue.SetActive(true);
        _settingMenue.SetActive(false);
    }
    public void AllignPixelImage(Vector2 camPos, float curScreenSize, float _renderTextureHeight)
    {
        float sizeConst = _renderTextureHeight / (curScreenSize * 16);
        Debug.Log(sizeConst);
        //_pixelatedImage.rectTransform.anchoredPosition = new Vector2(camPos.x % sizeConst, camPos.y % sizeConst);
    }

    public void UpdateGemCount(int num)
    {
        _gemCountTxt.text = num.ToString();
    }
}