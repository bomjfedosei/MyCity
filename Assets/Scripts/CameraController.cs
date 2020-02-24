using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Vector2 startPos;
    public int borderXUp, borderXDown, borderYUp, borderYDown;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) startPos = cam.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(0))
        {
            float posX = cam.ScreenToWorldPoint(Input.mousePosition).x - startPos.x;
            float posY = cam.ScreenToWorldPoint(Input.mousePosition).y - startPos.y;
            Vector3 pos = new Vector3(transform.position.x - posX, transform.position.y - posY, transform.position.z);
            transform.position = pos;
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            if (x < borderXUp || x > borderXDown ||
                y < borderYUp || y > borderYDown)
            {
                GetComponent<Map>().GenMap(x, y);
            }
        }
    }
}
