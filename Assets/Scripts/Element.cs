﻿using UnityEngine;
using Leguar.TotalJSON;

public class Element : MonoBehaviour
{
    public float marginY;
    public string type;
    private string key;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + marginY);
    }

    public void setJSON(JSON elementData){
        this.key = elementData.GetString("uuid");
        name = key;
        type = elementData.GetString("name");
        if (elementData.GetString("type") == "pawn")
        {
            if (elementData.ContainsKey("action"))
            {
                GetComponent<Pawn>().DrawAction(elementData.GetJSON("action"));
            }
            else
            {
                GetComponent<Pawn>().DropAction();
            }
        }
    }

    public string getKey(){
        return this.key;
    }
}
