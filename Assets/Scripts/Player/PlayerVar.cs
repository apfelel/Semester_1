using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVar : MonoBehaviour
{
    private CapsuleCollider2D _col;
    private Rigidbody2D _rb;
    [Header("Hook Things")]
    public float HookMaxRange;
    public GameObject HookAim;
    [HideInInspector]
    public bool IsHooked;
    [HideInInspector]
    public List<GameObject> AnchorsInScene = new List<GameObject>();

    [Header("Grounded Things")]
    public float CoyoteTime;
    public Transform GroundedRayPos;
    public LayerMask GroundLayer;
    
    [HideInInspector]
    public float LastGrounded = 100;
    [HideInInspector]
    public bool IsGroundedBuffered
    {
        get
        {
            if (LastGrounded < CoyoteTime)
                return true;
            return false;
        }
    }
    private bool _isGrounded;
    [HideInInspector]
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            if (value == true)
                LastGrounded = 0;
            _isGrounded = value;
        }
    }
    [HideInInspector]
    public float GroundDeny;

    [Header("Jump Things")]

    private bool _jumped;
    [HideInInspector]
    public bool Jumped
    {
        get => _jumped;
        set
        {
            _jumped = value;
            LastGrounded = 500;
        }
    }
    public float JumpBuffer;

    [Header("Wall Things")]
    public LayerMask WallHitLayer;
    public Transform WallUpperRayPos;
    public Transform WallLowerRayPos;
    public bool IsWallsliding
    {
        get
        {
            return _isWallsliding;
        }
        set
        {
            if (value == true)
                LastWallSlide = 0;
            _isWallsliding = value;
        }
    }
    [HideInInspector]
    public float LastWallSlide;
    private bool _isWallsliding;
    [HideInInspector]
    public bool IsWallSlidingBuffered
    {
        get
        {
            if (LastWallSlide < CoyoteTime)
                return true;
            return false;
        }
    }
    [HideInInspector]
    public bool IsWallInFront;
    [HideInInspector]
    public Vector2 LastWallHitPos;

    [Header("Speed Things")]
    public float TerminalVelY;


    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<CapsuleCollider2D>();
        GroundedRayPos.transform.localPosition = new Vector2(0, _col.offset.y - _col.size.y * 0.5f);
        WallUpperRayPos.transform.localPosition = new Vector2(_col.size.x / 2 + _col.offset.x, _col.offset.y + 0.2f);
        WallLowerRayPos.transform.localPosition = new Vector2(_col.size.x / 2 + _col.offset.x, _col.offset.y - 0.2f);
        LastGrounded = 500;
        LastWallSlide = 500;
        _rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        HookAim.transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
    private void FixedUpdate()
    {
        GroundDeny -= Time.deltaTime;
        LastGrounded += Time.deltaTime;
        LastWallSlide += Time.deltaTime;
        IsGrounded = false;
        IsWallInFront = false;

        if (Physics2D.Raycast(GroundedRayPos.position, Vector2.down, 0.1f, GroundLayer) & !Jumped && _rb.velocity.y < 0.01f && GroundDeny < 0)
            IsGrounded = true;
        RaycastHit2D hitLow = Physics2D.Raycast(WallLowerRayPos.position, transform.right, 0.3f, WallHitLayer);
        RaycastHit2D hitHigher = Physics2D.Raycast(WallUpperRayPos.position, transform.right, 0.3f, WallHitLayer);
        if (hitLow == true && hitHigher == true)
        {
            IsWallInFront = true;
            LastWallHitPos = hitLow.point;
        }
        if (Jumped && _rb.velocity.y < 0)
            Jumped = false;

    }

    private void OnDrawGizmosSelected()
    {
        _col = GetComponent<CapsuleCollider2D>();
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, _col.offset.y - _col.size.y * 0.5f), (Vector2)transform.position + new Vector2(0, _col.offset.y - _col.size.y * 0.5f) - new Vector2(0, 0.1f));
       
        Gizmos.DrawLine(
            (Vector2)transform.position + new Vector2(_col.size.x / 2 + _col.offset.x, _col.offset.y + 0.2f),
            (Vector2)transform.position + new Vector2(_col.size.x / 2 + _col.offset.x + 0.3f, _col.offset.y + 0.2f));
        Gizmos.DrawLine(
            (Vector2)transform.position + new Vector2(_col.size.x / 2 + _col.offset.x, _col.offset.y - 0.2f),
            (Vector2)transform.position + new Vector2(_col.size.x / 2 + _col.offset.x + 0.3f, _col.offset.y - 0.2f));

    }
}
