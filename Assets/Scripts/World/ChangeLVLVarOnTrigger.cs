using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLVLVarOnTrigger : MonoBehaviour
{
    [SerializeField]
    private float _newCamSize;
    [SerializeField]
    private bool _weakenedState;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.SetWeakenedState(_weakenedState);
            if(_newCamSize > 0)
                GameManager.Instance.ChangeScreensize(_newCamSize);
        }
    }
}
