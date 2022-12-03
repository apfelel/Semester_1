using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenueManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.Activate();
        GameManager.Instance.Weakend = true;
        SceneManager.LoadScene("Story");
    }
    public void Quit()
    { 
        Application.Quit();
    }
}
