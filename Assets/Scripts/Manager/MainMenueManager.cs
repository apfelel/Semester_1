using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenueManager : MonoBehaviour
{
    [SerializeField]
    private Button _start;

    private GameObject _lastActive;

    [SerializeField]
    private GameObject SelectUI, _levelMenu;
    private void Start()
    {
        _start.Select();
        GameManager.Instance.ResetValues();
    }
    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            _lastActive.GetComponent<Selectable>().Select();
            Debug.Log(_lastActive);
        }
        if (!UIManager.Instance.InSetting &! _levelMenu.activeInHierarchy)
        {
            _lastActive = EventSystem.current.currentSelectedGameObject;
            SelectUI.transform.position = Vector3.Lerp(SelectUI.transform.position, EventSystem.current.currentSelectedGameObject.transform.position, 0.1f);
        }
    }
    public void StartGame()
    {
        GameManager.Instance.Weakend = true;
        SceneManager.LoadScene("Story");
    }
    public void Quit()
    { 
        Application.Quit();
    }

    public void OpenLevelSelect()
    {
        _levelMenu.SetActive(true);
        _levelMenu.GetComponentInChildren<Selectable>().Select();
    }
    public void CloseLevelSelect()
    {
        _levelMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void LoadLevel(string lvl)
    {
        if(lvl == "L_0")
        {
            GameManager.Instance.Weakend = true;
        }
        GameManager.Instance.Activate();
        GameManager.Instance.SpawnedIn = true;
        SceneManager.LoadScene(lvl);
    }

    public void Settings()
    {
        UIManager.Instance.OpenSettings();
    }
}
