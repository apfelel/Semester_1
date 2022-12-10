using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private List<Sprite> _pics;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        for(int i = 0; i < _pics.Count; i++)
        {
            _image.sprite = _pics[i];
            yield return new WaitForSeconds(2f);
        }
        GameManager.Instance.Activate();
        SceneManager.LoadScene("L_0");
    }
}
