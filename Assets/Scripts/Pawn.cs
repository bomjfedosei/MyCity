using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Leguar.TotalJSON;


public class Pawn : MonoBehaviour
{
    void OnMouseDown(){
        string key = GetComponent<Element>().getKey();
        JSON body = new JSON();
        body.Add("object_uuid", key);
        body.Add("token", "b682277a53e4a6875ac34863ddbd8b1224430da7a74d665086c9433f921f9b9d");
        //StartCoroutine(Send.Request("get_map", body.CreateString(), ActionsView));
    }

    void ActionsView(string response){
        Debug.Log(response);
    }
    
}
