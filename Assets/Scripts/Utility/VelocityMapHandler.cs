using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMapHandler : MonoBehaviour
{
    private Rigidbody2D _rb;
    private ParticleSystem.MainModule _ps;
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponentInChildren<ParticleSystem>().main;
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Color col;
        if(_rb.velocity.x > 2f)
            col = Color.red;
        else if(_rb.velocity.x < -2f)
            col = Color.blue;
        else
            col = Color.black;

        col.a = Mathf.Abs(_rb.velocity.x) / 10;
        _ps.startColor = col;
    }   
}
