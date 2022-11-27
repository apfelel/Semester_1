using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private Sprite _onSprite, _offSprite;
    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameManager.Instance.NewSpawnPoint(this);
        }
    }
    public void ChangeState(bool active)
    {
        _sr.sprite = active ? _offSprite : _offSprite;
    }
}
