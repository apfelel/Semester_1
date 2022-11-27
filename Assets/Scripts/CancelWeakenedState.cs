using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelWeakenedState : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.SetWeakenedState(false);
            GameManager.Instance.ChangeScreensize(11);
        }
    }
}
