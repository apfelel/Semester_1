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
            var pc = GameManager.Instance.PlayerController;
            //pc.Rb.AddForce((_direction == Vector2.zero ? (Vector2)transform.up : _direction.normalized) * _force);
            pc.Rb.velocity = _direction * _force;
            pc.SkipGroundedBuffer();
            GameManager.Instance.PlayerVar.HasDash = true;
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
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 3));
    }
}
