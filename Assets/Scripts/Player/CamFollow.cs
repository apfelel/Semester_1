using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + Offset, 0.07f);
    }

    public string DestroyAnimation;
    private IEnumerator DestroyDelay()
    {
        GetComponent<Animator>().Play(DestroyAnimation);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

    }
}
