using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleParticle : MonoBehaviour
{
    float _time;

    private enum State
    {
        Spread, Wait, Target, Death
    }
    private State _curState = State.Spread;

    [SerializeField]
    private float _spreadTime, _waitTime;
    [SerializeField]
    private float _speed;
    GameObject _target;

    private Vector2 _rng;
    // Start is called before the first frame update
    void Start()
    {
        _rng = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
        _target = GameManager.Instance.Player;

        _spreadTime *= Random.Range(0, 1f);
        _speed *= Random.Range(0.7f, 1.3f);

        transform.Rotate(new Vector3(0, 0, Random.Range(-45, 45)));
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        transform.position += new Vector3(Mathf.PerlinNoise(_rng.x++, _rng.y++) - 0.5f, Mathf.PerlinNoise(_rng.y, _rng.x) - 0.5f) * Time.deltaTime * 2;
        switch(_curState)
        {
            case State.Spread:
                if (_time > _spreadTime)
                    _curState = State.Wait;
                transform.position += transform.up * _speed * Time.deltaTime * 20;
                break;

            case State.Wait:
                if (_time > _spreadTime + _waitTime)
                    _curState = State.Target;

                break;

            case State.Target:
                var walkVector = (_target.transform.position + _target.transform.up - transform.position).normalized * _speed * Time.deltaTime * 20;
                transform.position += walkVector * (1 + (_time - _spreadTime + _waitTime) * 0.5f);

                if (walkVector.magnitude > Vector2.Distance(transform.position, _target.transform.position + _target.transform.up))
                {
                    _curState = State.Death;
                }
                break;
            case State.Death:
                GameManager.Instance.Collectibles += 1;
                Destroy(gameObject);
                break;
        }
        
        
    }
}
