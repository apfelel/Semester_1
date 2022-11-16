using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private bool _challenge;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (_challenge)
                GameManager.Instance.ChallengeCollectibles++;
            else
                GameManager.Instance.Collectibles++;

            SoundManager.Instance.CrystalPickup();

            Destroy(gameObject);
        }
    }
}
