using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlobalLight : MonoBehaviour
{
    [SerializeField]
    private bool _toOutside;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        GlobalLightManager.Instance.State = _toOutside;
    }
}
