using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncWorldToScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private Vector2 _offset;
    void Update()
    {
        var newPos = Camera.main.ScreenToWorldPoint(_target.transform.position) + (Vector3)_offset;

        newPos.z = 0;

        transform.position = newPos;
    }
}
