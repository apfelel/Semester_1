using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject loop;
    private float _width;
    private float _camwidth;
    private List<GameObject> ParallaxForeground = new();
    private GameObject _cam;
    public Vector2 Strength;


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main.gameObject;
        _width = loop.GetComponent<SpriteRenderer>().bounds.size.x * 16;
        Debug.Log(_width);
        _camwidth = Camera.main.orthographicSize * 32 * (16f / 9);
        float i = (_camwidth / _width);
        Debug.Log(i);
        Debug.Log(_camwidth);
        for (int x = 0; x < i + 2; x++)
        {
            var gb = Instantiate(loop, transform.position, transform.rotation, transform);
            ParallaxForeground.Add(gb);
            gb.transform.localPosition = new Vector3((_width * x / 16) - (_camwidth / 32), 0, 0);
        }
        loop.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3((_cam.transform.position.x * Strength.x) % (_width / 16) + _cam.transform.position.x, _cam.transform.position.y * Strength.y);
    }
}