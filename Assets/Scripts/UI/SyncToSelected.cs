using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SyncToSelected : MonoBehaviour
{
    [SerializeField]
    private int _mod = 1;
    private SelectOnHover _selected;

    private GameObject _selectedGameObject;
    void Update()
    {
        if (_selectedGameObject != EventSystem.current.currentSelectedGameObject)
        {
            _selectedGameObject = EventSystem.current.currentSelectedGameObject;
            _selected = _selectedGameObject.GetComponent<SelectOnHover>();
            transform.position = EventSystem.current.currentSelectedGameObject.transform.position + ((_selected? _selected.SelectionMult: Vector3.one) * _mod);
        }

    }
}
