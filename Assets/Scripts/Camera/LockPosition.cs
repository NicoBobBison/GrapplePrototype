using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    [SerializeField] bool lockXPosition;
    [SerializeField] bool lockYPosition;
    Vector2 originalPos;
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockXPosition)
        {
            Vector2 temp = transform.position;
            temp.x = originalPos.x;
            transform.position = temp;
        }
        if (lockYPosition)
        {
            Vector2 temp = transform.position;
            temp.y = originalPos.y;
            gameObject.transform.position = temp;
        }
    }
}
