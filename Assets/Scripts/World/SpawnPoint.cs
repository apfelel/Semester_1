using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private Sprite _onSprite, _offSprite;
    [SerializeField]
    private GameObject _light;

    private bool _active;
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
        if (active == _active) return;
        _active = active;
        if (active)
        {
            _sr.sprite = _onSprite;
            _light.SetActive(true);
            SoundManager.Instance.PlaySound("Campfire");
        }
        else
        {
            _sr.sprite = _offSprite;
            _light.SetActive(false);
        }
    }
}
