using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private GameObject _pauseMenue;
    [SerializeField]
    private GameObject _settingMenue;
    private void Start()
    {
        _pauseMenue.SetActive(false);
        _settingMenue.SetActive(false);
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
            UnPause();
        else
            Pause();
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
}
