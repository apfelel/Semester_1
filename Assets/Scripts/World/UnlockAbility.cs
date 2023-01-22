using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAbility : MonoBehaviour
{
    [SerializeField]
    private bool _unlockGrapple;
    [SerializeField]
    private bool _unlockGlove;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySound("PickUp", 0.2f);

        if (_unlockGrapple)
            GameManager.Instance.UnlockGrapple();
        if (_unlockGlove)
            GameManager.Instance.UnlockGloves();
        Destroy(gameObject);
    }
}
