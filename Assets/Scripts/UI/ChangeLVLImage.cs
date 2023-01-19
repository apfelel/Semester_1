using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeLVLImage : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _images;

    [SerializeField]
    private Image _image;
    private GameObject _curSelected;
    [SerializeField]
    private GameObject _buttonParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_curSelected != EventSystem.current.currentSelectedGameObject)
        {
            _curSelected = EventSystem.current.currentSelectedGameObject;
            for (int i = 0; i < _buttonParent.transform.childCount; i++)
            {
                Debug.Log(i);
                if(_curSelected == _buttonParent.transform.GetChild(i).gameObject)
                {
                    _image.sprite = _images[i];
                    break;
                }
            }
        }
            
    }
}
