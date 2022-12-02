using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedScreenChange : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeScene(3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(_sceneName);
    }
}
