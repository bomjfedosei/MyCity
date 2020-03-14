using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leguar.TotalJSON;
using Tools;

public class Structure : MonoBehaviour
{
    public string resourceName;
    void OnMouseDown(){
        string key = GetComponent<Element>().getKey();
        JSON body = new JSON(); 
        body.Add("token", "b682277a53e4a6875ac34863ddbd8b1224430da7a74d665086c9433f921f9b9d");
        body.Add("resource", resourceName);
        StartCoroutine(Send.Request("get_player_resources", body.CreateString(), ShowRes));
    }

    void ShowRes(string response){
        JSON responseJSON = JSON.ParseString(response);
        int count = responseJSON.GetJArray("resources").GetJSON(0).GetInt("wood");
        GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(Camera.GetComponent<ResouceText>().showSprite(resourceName, count));
    }
}
