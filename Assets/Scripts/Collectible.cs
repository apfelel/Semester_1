using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private int _count;
    [SerializeField]
    private GameObject _particle;
    [SerializeField]
    private Sprite _destroyedSprite;
    [SerializeField]
    private Vector2 _particleDirection, _spawnOffset;
    [SerializeField]
    private float _randomSpawn;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            for (int i = 0; i < _count; i++)
            {
                var gb = Instantiate(_particle);
                gb.transform.up = _particleDirection;
                gb.transform.position = transform.position + (Vector3)_spawnOffset + new Vector3(Random.Range(-_randomSpawn, _randomSpawn), Random.Range(-_randomSpawn, _randomSpawn));
            }
            _sr.sprite = _destroyedSprite;
            Destroy(GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (Vector3)_spawnOffset, 0.1f);

        Gizmos.DrawWireSphere(transform.position + (Vector3)_spawnOffset, _randomSpawn);
    }
}
