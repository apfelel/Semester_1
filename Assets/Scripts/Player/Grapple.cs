using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    [HideInInspector]
    public GameObject CurAnchor;
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
    // Start is called before the first frame update
    void Start()
    {
        _playerVar = GetComponent<PlayerVar>();
        _rb = GetComponent<Rigidbody2D>();
        _playerVar.AnchorsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor"));
        _ropeLine = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_joint != null)
        {
            DrawRope();
            if (!_drawingIn)
            {
                float distanceFromPoint = Vector2.Distance(transform.transform.position, CurAnchor.transform.position);
                _joint.distance = Mathf.Min(_shortestDist, distanceFromPoint);
                _shortestDist = _joint.distance;
            }
            else
            {
                _joint.distance -= Time.deltaTime * _drawingInSpeed;
                if(_rb.velocity.magnitude > _playerVar.TerminalVelY)
                {
                    _rb.velocity = _playerVar.TerminalVelY / _rb.velocity.magnitude * _rb.velocity;
                }
            }
        }

        if(_playerVar.IsHooked)
        {
            if(!CheckLOS())
            {
                EndGrapple();
            }
        }
    }
    public void StartGrapple()
    {
        CurAnchor = GetAvailableHook();
        if (CurAnchor == null) return;
        if (!CheckLOS()) return;
        _joint = gameObject.AddComponent<SpringJoint2D>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.autoConfigureDistance = false;
        _joint.connectedAnchor = CurAnchor.transform.position;
        _shortestDist = Vector2.Distance(transform.position, CurAnchor.transform.position) * 0.95f;
        _joint.distance = _shortestDist;
        _joint.enableCollision = true;
        _joint.dampingRatio = 0.8f;
        _joint.frequency = 0.8f;
        _rb.velocity = _rb.velocity * 1.10f;
        _ropeLine.positionCount = 2;

        _playerVar.IsHooked = true;
    }
    public void EndGrapple()
    {
        _rb.velocity = _rb.velocity * 1.10f;
        Destroy(GetComponent<SpringJoint2D>());
        _ropeLine.positionCount = 0;
        _playerVar.IsHooked = false;
    }

    private void DrawRope()
    {
        _ropeLine.SetPosition(0, _ropeLine.transform.position);
        _ropeLine.SetPosition(1, CurAnchor.transform.position);
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
            var temp = Vector2.Distance(anchor.transform.position, _playerVar.HookAim.transform.position);
            if (Vector2.Distance(anchor.transform.position, _playerVar.HookAim.transform.position) < nearestDist &&
                Vector2.Distance(anchor.transform.position, transform.position) < _playerVar.HookMaxRange)
            {
                nearestDist = temp;
                nearestGb = anchor;
            }
        }
        return nearestGb;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EndGrapple();
        _playerVar.IsHooked = false;
    }

    private bool CheckLOS()
    {
        var ptoa = CurAnchor.transform.position - transform.position;
        return !Physics2D.Raycast(transform.position, ptoa, ptoa.magnitude, _playerVar.GroundLayer);
    }
}
