﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Vector2 startPos, previosPos;
    public int borderXUp, borderXDown, borderYUp, borderYDown;
    public Text coorsInfo;
    private bool isLead;
    private GameObject Leader;


    private void Start()
    {
        cam = GetComponent<Camera>();
        setCoorsText();
        GoStartPos();
        isLead = false;
    }

    private void Update()
    {
        if (!isLead)
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
            if (previosPos != (Vector2)transform.position)
            {
                setCoorsText();
            }
        }
        else
        {
            transform.position = Leader.transform.position + new Vector3(0f, 0f, -10f);
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            if (x < borderXUp || x > borderXDown ||
                y < borderYUp || y > borderYDown)
            {
                GetComponent<Map>().GenMap(x, y);
            }
            setCoorsText();
        }

    }

    public void SetLead(GameObject target)
    {
        isLead = true;
        Leader = target;
    }

    public void SetUnLead()
    {
        isLead = false;
    }

    private void setCoorsText()
    {
        string posX = ((int)transform.position.x).ToString();
        string posY = ((int)transform.position.y).ToString();
        coorsInfo.text = "X: " + posX + " Y: " + posY;
    }

    private void GoStartPos()
    {
        transform.position = new Vector3(
            PlayerPrefs.GetInt("spawn_x"), 
            PlayerPrefs.GetInt("spawn_y"), 
            -10);
    }
}
