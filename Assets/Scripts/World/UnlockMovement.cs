using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMovement : MonoBehaviour
{
    [SerializeField]
    private string _newMusic;
    [SerializeField]
    private float _newCamSize;
    [SerializeField]
    private bool _weakenedState;
    [SerializeField]
    Transform _teleportOnEnd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(StartSequence());
        }
    }
    private IEnumerator StartSequence()
    {
        SoundManager.Instance.PlayMusic(_newMusic);
        GameManager.Instance.PlayerCinematic.Wait(2);

        yield return new WaitForSeconds(2);
        GameManager.Instance.PlayerCinematic.MoveXSec(1, 2, 0.5f);

        yield return new WaitForSeconds(1);
        UIManager.Instance.FadeOut();

        yield return new WaitForSeconds(1);
        GameManager.Instance.Player.transform.position = _teleportOnEnd.position;
        UIManager.Instance.FadeIn();
        GameManager.Instance.SetWeakenedState(_weakenedState);
        GameManager.Instance.ChangeScreensize(_newCamSize);
        GameManager.Instance.PlayerCinematic.Wait(1);  
    }
}
