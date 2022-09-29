using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    private GameObject _curAnchor;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    public LineRenderer RopeLine;
    private SpringJoint2D _joint;
    private float _shortestDist;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawRope();
        if(_joint != null)
        {
            float distanceFromPoint = Vector2.Distance(transform.transform.position, _curAnchor.transform.position);
            _joint.distance = Mathf.Min(_shortestDist, distanceFromPoint);
            _shortestDist = _joint.distance;
        }
    }

    public void StartGrapple(InputAction.CallbackContext obj)
    {
        _curAnchor = GetAvailableHook();
        if (_curAnchor == null) return;
        _playerController.IsHooked = true;
        _joint = gameObject.AddComponent<SpringJoint2D>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _curAnchor.transform.transform.position;
        _shortestDist = Vector2.Distance(transform.transform.position, _curAnchor.transform.position) * 0.95f;
        _joint.distance = _shortestDist;
        _joint.enableCollision = true;
        _joint.dampingRatio = 10;
        _joint.frequency = 0.8f;
        _rb.velocity = _rb.velocity * 1.10f;
        RopeLine.positionCount = 2;
    }

    public void EndGrapple(InputAction.CallbackContext obj)
    {
        _playerController.IsHooked = false;
        _rb.velocity = _rb.velocity * 1.10f;
        Destroy(GetComponent<SpringJoint2D>());
        RopeLine.positionCount = 0;
    }

    private void DrawRope()
    {
        if (_playerController.IsHooked == true)
        {
            RopeLine.SetPosition(0, transform.transform.position);
            RopeLine.SetPosition(1, _curAnchor.transform.position);
        }
    }

    public GameObject GetAvailableHook()
    {
        float nearestDist = int.MaxValue;
        GameObject nearestGb = null;
        foreach(var anchor in _playerController.AnchorsInScene)
        {
            var temp = Vector2.Distance(anchor.transform.transform.position, transform.transform.position);
            if (Vector2.Distance(anchor.transform.transform.position, transform.transform.position) < nearestDist)
            {
                nearestDist = temp;
                nearestGb = anchor;
            }
        }
        return nearestGb;
    }
}
