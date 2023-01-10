using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, IPointerEnterHandler
{
    private Selectable _selectable;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectable.Select();
    }

    // Start is called before the first frame update
    void Start()
    {
        _selectable = GetComponent<Selectable>();
    }
}
