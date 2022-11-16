using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenueManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.Activate();
        SceneManager.LoadScene("E_0");
    }
    public void Quit()
    { 
        Application.Quit();
    }
}
