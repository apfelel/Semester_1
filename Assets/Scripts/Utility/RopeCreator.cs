using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{

    [SerializeField]
    private GameObject _start;
    [SerializeField]
    private bool _startStatic;

    [SerializeField]
    private GameObject _end;
    [SerializeField]
    private bool _endStatic;

    private LineRenderer _lr;

    [SerializeField]
    private float _interval, _lengthDiff;

    public List<Transform> _links;

    private GameObject _lastGb;
    // Start is called before the first frame update
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        CalculateRope();
    }
    private void CalculateRope()
    {
        for (int i = 0; i < _links.Count; i++)
        {
            Debug.Log("s");
            _lr.SetPosition(i, _links[i].gameObject.transform.position);
        }
    }

    [Button("Generate Rope")]
    public void GenerateRope()
    {
        _lastGb = _start;
        _lr = GetComponent<LineRenderer>();

        _lr.positionCount = 0;
        _links = new List<Transform>();
        Vector3 curPos = _start.transform.position;
        AddLink(_start,_lastGb, curPos, _startStatic);
        while(Vector2.Distance(curPos, _end.transform.position) > _interval)
        {
            
            curPos += (_end.transform.position - _start.transform.position).normalized * _interval;
            AddLink(new GameObject(),_lastGb, curPos);
        }
        AddLink(_end, _lastGb, _end.transform.position, _endStatic);
    }

    private void AddLink(GameObject gb, GameObject lastGb, Vector3 pos, bool isstatic = false)
    {
        

        var joint = gb.AddComponent<SpringJoint2D>();
        joint.frequency = 10;
        joint.dampingRatio = 1;
        joint.connectedBody = lastGb.GetComponent<Rigidbody2D>();
        joint.autoConfigureDistance = false;
        joint.distance += _lengthDiff;
        gb.transform.position = pos;
        gb.transform.SetParent(transform);
        gb.name = _links.Count.ToString();
        if (isstatic)
            gb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        _lastGb = gb;
        _links.Add(gb.transform);

        _lr.positionCount++;
        _lr.SetPosition(_lr.positionCount - 1, _lastGb.transform.position);
    }

    [Button("Remove Rope")]
    public void RemoveRope()
    {
        _links.ForEach((l) => DestroyImmediate(l));
        _links = new List<Transform>();

        var gb = new GameObject();
        gb.transform.SetParent(transform);
        gb.name = "Start";
        gb.transform.position = transform.position;
        _start = gb;

        var gb2 = new GameObject();
        gb2.transform.SetParent(transform);
        gb2.name = "End";
        gb2.transform.position = transform.position;
        _end = gb2;
    }
}
