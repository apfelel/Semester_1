using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private string _lvlName;
    public GameManager.Direction Direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (GameManager.Instance.CheckIfLeaving(Direction))
                GameManager.Instance.LoadNextLevel(_lvlName, Direction);
        }
    }
}
