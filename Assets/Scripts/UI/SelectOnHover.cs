using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerUpHandler
{
    public Vector3 SelectionMult = Vector3.zero;

    private bool _playUp;
    private Selectable _selectable;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectable.Select();
    }
    // Start is called before the first frame update
    void Start()
    {
        _selectable = GetComponent<Selectable>();
        _playUp = GetComponent<Slider>() == null;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (_playUp)
            SoundManager.Instance.PlaySound("ButtonClick", 0.7f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonHover", 0.15f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_playUp)
            SoundManager.Instance.PlaySound("ButtonClick", 0.7f);
    }
}
