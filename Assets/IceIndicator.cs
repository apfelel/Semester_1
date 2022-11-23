using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceIndicator : MonoBehaviour
{
    public Transform GroundCheck1, GroundCheck2;
    public LayerMask HitLayer, WaterLayer;
    private BoxCollider2D _boxCollider;

    public RaycastHit2D[] hits = new RaycastHit2D[2];
    // Start is called before the first frame update
    void Awake()
    {
        hits[0] = new RaycastHit2D();
        hits[1] = new RaycastHit2D();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool CheckIfValid()
    {
        hits[0] = Physics2D.Raycast(GroundCheck1.transform.position, Vector2.down, 0.5f, HitLayer);
        hits[1] = Physics2D.Raycast(GroundCheck2.transform.position, Vector2.down, 0.5f, HitLayer);

        Collider2D[] col = new Collider2D[10];

        var filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(HitLayer);
        Debug.Log(col[0]?.name);
        Debug.Log(Physics2D.OverlapCollider(_boxCollider, filter, col));
        Debug.Log(ValidHits());
        if (Physics2D.OverlapCollider(_boxCollider, filter, col) == 0 && ValidHits())
        {
            Debug.Log("y");
            return true;
        }
        Debug.Log("n");
        return false;
    }

    public bool CheckIfWater()
    {

        if (hits[0] && hits[1])
        {
            if (Mathf.Pow(hits[0].transform.gameObject.layer, 2) == WaterLayer && Mathf.Pow(hits[1].transform.gameObject.layer, 2) == WaterLayer) return true;
        }
        return false;
    }

    public bool ValidHits()
    {
        if(hits[0] && hits[1])
            if (hits[0].transform.gameObject.layer == hits[1].transform.gameObject.layer) return true;
        return false;
    }
}
