using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private Renderer _image;
    [SerializeField]
    private List<string> _texts;
    [SerializeField]
    private TextMeshProUGUI _bottomText;

    [SerializeField]
    private GameObject _particle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        int textIndex = 0;
        for(int i = 0; i < _texts[textIndex].Length; i++)
        {
            _bottomText.text += _texts[textIndex][i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(4);


        textIndex++;
        _particle.SetActive(true);
        StartCoroutine(Smelt());
        _bottomText.text = "";
        for (int i = 0; i < _texts[textIndex].Length; i++)
        {
            _bottomText.text += _texts[textIndex][i];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(100f);
        GameManager.Instance.Activate();
        SceneManager.LoadScene("L_0");
    }

    private IEnumerator Smelt()
    {
        for(int i = 0; i < 2000; i++)
        {
            _image.material.SetFloat("_Down", i / 100f);
            yield return new WaitForSeconds(0.04f);
        }
    }
}
