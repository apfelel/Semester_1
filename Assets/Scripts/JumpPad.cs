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
            var pc = collision.GetComponent<PlayerController>();
            var pv = collision.GetComponent<PlayerVar>();
            if (!pv.Jumped)
            {
                pc.Rb.velocity = new Vector2(pc.Rb.velocity.x, 0);
                pc.Rb.AddForce((_direction == Vector2.zero ? (Vector2)transform.up : _direction) * _force);
                pc.SkipGroundedBuffer();
            }
        }
    }

    [Button("Local Up")]
    [HorizontalGroup("Split", 0.2f)]
    private void ResetDir()
    {
        _direction = Vector2.zero;
    }
}
