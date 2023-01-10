using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject _cam;
    public Vector2 Strength;

    private Vector2 _pos;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _cam.transform.position * Strength;    
    }
}