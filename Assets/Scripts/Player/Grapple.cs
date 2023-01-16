using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    [HideInInspector]
    private Rigidbody2D _rb;
    private LineRenderer _ropeLine;
    private SpringJoint2D _joint;
    private float _shortestDist;
    private bool _drawingIn;

    [SerializeField]
    private float _drawingInSpeed;
    [SerializeField]
    private float _drawingInVelLimit;

    private PlayerVar _playerVar;

    public List<Vector3> RopeSegments = new List<Vector3>();

    public enum Dir
    {
        Left, Right
    }
    public Vector3 CurAnchor 
    { 
        get
        {
            return RopeSegments[RopeSegments.Count - 1];
        } 
    }

    public Vector3 LastAnchor
    {
        get
        {
            return RopeSegments[RopeSegments.Count - 2];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _rb = GetComponent<Rigidbody2D>();
        _playerVar.AnchorsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor"));
        _ropeLine = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_joint != null)
        {
            if (_playerVar.IsGrounded)
            {
                EndGrapple();
                return;
            }

            var curAnchHit = CheckLOS(CurAnchor);
            if(curAnchHit)
            {
                RopeSegments.Add(curAnchHit.point);
                _ropeLine.positionCount++;
                _joint.connectedAnchor = curAnchHit.point;
                _shortestDist = (transform.position - CurAnchor).magnitude - 1f;
            }

            if (RopeSegments.Count > 1)
            {
                var lastAnchHit = CheckLOS(LastAnchor);
                if (!lastAnchHit)
                {
                    RopeSegments.RemoveAt(RopeSegments.Count - 1);
                    _ropeLine.positionCount--;
                    _joint.connectedAnchor = CurAnchor;

                    _shortestDist = (transform.position - CurAnchor).magnitude - 1f;
                    _joint.distance = _shortestDist;
                }
            }

            if (!_drawingIn)
            {
                float distanceFromPoint = Vector2.Distance(transform.transform.position, CurAnchor);
                _joint.distance = Mathf.Min(_shortestDist, distanceFromPoint);
                _shortestDist = _joint.distance;
            }
            else
            {
                if (_joint.distance > _ropeLine.transform.localPosition.y)
                {
                    _shortestDist -= Time.deltaTime * _drawingInSpeed;
                    _joint.distance = _shortestDist;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if(_joint != null)
            DrawRope();
    }
    public void StartGrapple()
    {
        var anchor = GetAvailableHook();
        if (anchor == null) return;
        RopeSegments.Add(anchor.transform.position);
        if (RopeSegments.Count == 0) return;

        _joint = gameObject.AddComponent<SpringJoint2D>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.autoConfigureDistance = false;
        _joint.connectedAnchor = CurAnchor;
        _shortestDist = Vector2.Distance(transform.position, CurAnchor) * 0.95f;
        _joint.distance = _shortestDist;
        _joint.enableCollision = true;
        _joint.dampingRatio = 0.5f;
        _joint.frequency = 0.9f;
        _rb.velocity = _rb.velocity * 1.10f;
        _ropeLine.positionCount = 2;

        _playerVar.IsHooked = true;
    }
    public void EndGrapple()
    {
        RopeSegments = new List<Vector3>();
        _rb.velocity = _rb.velocity * 1.10f;
        Destroy(GetComponent<SpringJoint2D>());
        _ropeLine.positionCount = 0;
        _playerVar.IsHooked = false;
    }

    private void DrawRope()
    {
        for(int i = 0; i < RopeSegments.Count; i++)
        {
            _ropeLine.SetPosition(i, RopeSegments[i]);
        }
        _ropeLine.SetPosition(RopeSegments.Count, _ropeLine.transform.position);
    }

    public void StartDrawingIn()
    {
        _drawingIn = true;
    }
    public void StopDrawingIn()
    {
        _drawingIn = false;
    }
    private GameObject GetAvailableHook()
    {
        float nearestDist = int.MaxValue;
        GameObject nearestGb = null;
        foreach (var anchor in _playerVar.AnchorsInScene)
        {
            var dist = Vector2.Distance(anchor.transform.position, transform.position);
            if (dist < _playerVar.HookMaxRange)
            {
                if (dist < nearestDist)
                {
                    if (CheckLOS(anchor.transform.position)) continue;
                    nearestDist = dist;
                    nearestGb = anchor;
                }
            }
        }
        return nearestGb;
    }

    private RaycastHit2D CheckLOS(Vector3 target)
    {
        var ptoa = target - transform.position;
        return Physics2D.Raycast(transform.position, ptoa, ptoa.magnitude * 0.999f, _playerVar.WallHitLayer);
    }
}
