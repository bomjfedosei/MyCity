using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public float marginY;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + marginY);
    }
}
