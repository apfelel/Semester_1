using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTypeTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _inCave;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.ChangeInCave(_inCave);
    }
}
