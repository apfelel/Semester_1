using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerUpHandler
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

    public void OnSubmit(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonClick", 0.7f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonHover", 0.15f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonClick", 0.7f);
    }
}
