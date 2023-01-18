using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicTrigger : MonoBehaviour
{
    [SerializeField]
    private string _song, _ambient;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_song != "")
            SoundManager.Instance.PlayMusic(_song);
        if(_song != "")
            SoundManager.Instance.PlayAmbient(_ambient);
        Destroy(this);
    }
}
