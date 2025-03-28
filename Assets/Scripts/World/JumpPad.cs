using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField]
    private float _force;
    [SerializeField]
    [HorizontalGroup("Split", 0.8f)]
    private Vector2 _direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound("JumpPad", 1f);
            var pc = GameManager.Instance.PlayerController;
            GameManager.Instance.PlayerVar.HasDash = true;
            GameManager.Instance.PlayerVar.IsDashing = false;
            pc.Rb.velocity = _direction * _force;
            pc.SkipGroundedBuffer();
           
        }
    }

    [Button("Local Up")]
    [HorizontalGroup("Split", 0.2f)]
    private void ResetDir()
    {
        _direction = transform.up;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + ((Vector3)_direction * 3));
    }
}
