using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxe : MonoBehaviour
{


    private float length, startpos;
    private float startposy;
    public GameObject rearsprite;
    public GameObject frontsprite;
    private GameObject cam;
    public float parallaxEffect;


    void Start()
    {

        startpos = transform.position.x;
        startposy = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        rearsprite.transform.localPosition = new Vector3(-length, 0, 0);
        frontsprite.transform.localPosition = new Vector3(length, 0, 0);
        cam = Camera.main.gameObject;

    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, startposy, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}