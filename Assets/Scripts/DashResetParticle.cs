using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashResetParticle : MonoBehaviour
{
    GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        var walkVector = (_target.transform.position + _target.transform.up - transform.position).normalized * 1 * Time.deltaTime * 20;
        transform.position += walkVector;

        if (walkVector.magnitude > Vector2.Distance(transform.position, _target.transform.position + _target.transform.up))
        {
            GameManager.Instance.PlayerVar.HasDash = true;
            Destroy(gameObject);
        }
    }
}
