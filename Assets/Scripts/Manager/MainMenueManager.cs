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
    private GameObject SelectUI;
    private void Start()
    {
        _start.Select();
    }
    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
            _lastActive.GetComponent<Button>().Select();
        if (!UIManager.Instance.InSetting)
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
}
