using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicTrigger : MonoBehaviour
{
    [SerializeField]
    private string _song;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlayMusic(_song);
        Destroy(this);
    }
}
