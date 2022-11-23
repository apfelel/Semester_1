using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlaceBlock : MonoBehaviour
{

    public float Range;
    public bool InIceMode;
    [Serializable]
    public struct Block
    {
        public GameObject Indicator, Iceblock;
    }
    public Block[] Blocks;

    private int _curForm;
    public LayerMask IceblockLayer;

    private IceIndicator _curIndicator;
    private SpriteRenderer _curIndicatorGFX;
    public int _waterCount = 3;

    private List<GameObject> _iceBlocksInWorld = new();
    private PlayerInputActions _playerInputActions;

    // Start is called before the first frame update
    void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        _iceBlocksInWorld = GameObject.FindGameObjectsWithTag("IceBlock").ToList();
    }
    private void Update()
    {
        MeltNearBlock();
        if (!InIceMode) return;
        if (_curIndicator == null)
        {
            _curIndicator = Instantiate(Blocks[_curForm].Indicator).GetComponent<IceIndicator>();
            _curIndicatorGFX = _curIndicator.GetComponentInChildren<SpriteRenderer>();
        }

        _curIndicator.transform.position = new Vector3(transform.position.x + Range * transform.localScale.x, transform.position.y, transform.position.z);
        if(_curIndicator.CheckIfValid())
        {
            _curIndicatorGFX.color = Color.white;
        }
        else
        {
            _curIndicatorGFX.color = Color.red;
        }

        if(_curIndicator.CheckIfWater())
        {
            _curIndicatorGFX.transform.localPosition = new Vector2(0, -0.7f);
        }
        else
        {
            _curIndicatorGFX.transform.localPosition = Vector2.zero;
        }
    }

    public void CancelIceMode()
    {
        InIceMode = false;
        Destroy(_curIndicator.gameObject);
    }
    public void ToggleIceMode()
    {
        if(InIceMode)
        {
            if (_curIndicator.CheckIfValid())
            {
                var block = Instantiate(Blocks[_curForm].Iceblock);
                _iceBlocksInWorld.Add(block);
                block.transform.position = _curIndicatorGFX.transform.position;
                _waterCount--;
            }
            Destroy(_curIndicator.gameObject);
            InIceMode = false;
        }
        else if (!InIceMode)
        {
            if(_waterCount > 0)
                InIceMode = true;
        }

    }

    public void ChangeBlockAShape(int num)
    {
        Destroy(_curIndicator);
        _curForm = num;
    }
    public bool MeltNearBlock()
    {
        float nearestHit = 900;
        GameObject nearestGb = null;
        foreach (var block in _iceBlocksInWorld)
        {
            var distance = (block.transform.position - transform.position).magnitude;
            if (distance < nearestHit)
            {
                nearestHit = distance;
                nearestGb = block;
            }
        }

        if (nearestHit < 3)
        {
            _iceBlocksInWorld.Remove(nearestGb);
            Destroy(nearestGb);
            _waterCount++;
            return true;
        }
        return false;
    }
    private void OnDisable()
    {
    }
}
