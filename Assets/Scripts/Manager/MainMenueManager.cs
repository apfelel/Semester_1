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
    private void Start()
    {
        _start.Select();
    }
    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
            _lastActive.GetComponent<Button>().Select();
        if(!UIManager.Instance.InSetting)
            _lastActive = EventSystem.current.currentSelectedGameObject;
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
}
