using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    private GameObject _curAnchor;
    private Rigidbody2D _rb;
    public LineRenderer RopeLine;
    private SpringJoint2D _joint;
    private float _shortestDist;
    private bool _drawingIn;

    [SerializeField]
    private float _drawingInSpeed;
    [SerializeField]
    private float _drawingInVelLimit;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_joint != null)
        {
            DrawRope();
            if (!_drawingIn)
            {
                float distanceFromPoint = Vector2.Distance(transform.transform.position, _curAnchor.transform.position);
                _joint.distance = Mathf.Min(_shortestDist, distanceFromPoint);
                _shortestDist = _joint.distance;
            }
            else
            {
                if(_rb.velocity.magnitude < _drawingInVelLimit)
                _joint.distance -= Time.deltaTime * _drawingInSpeed;
            }
        }
    }

    public void StartGrapple(GameObject anchor)
    {
        _curAnchor = anchor;
        if (_curAnchor == null) return;
        _joint = gameObject.AddComponent<SpringJoint2D>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.autoConfigureDistance = false;
        _joint.connectedAnchor = _curAnchor.transform.transform.position;
        _shortestDist = Vector2.Distance(transform.transform.position, _curAnchor.transform.position) * 0.95f;
        _joint.distance = _shortestDist;
        _joint.enableCollision = true;
        _joint.dampingRatio = 0.8f;
        _joint.frequency = 0.8f;
        _rb.velocity = _rb.velocity * 1.10f;
        RopeLine.positionCount = 2;
    }

    public void EndGrapple()
    {
        _rb.velocity = _rb.velocity * 1.10f;
        Destroy(GetComponent<SpringJoint2D>());
        RopeLine.positionCount = 0;
    }

    private void DrawRope()
    {
        RopeLine.SetPosition(0, transform.transform.position);
        RopeLine.SetPosition(1, _curAnchor.transform.position);
    }

    public void StartDrawingIn()
    {
        _drawingIn = true;
    }
    public void StopDrawingIn()
    {
        _drawingIn = false;
    }
    
}
