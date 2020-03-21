using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Tools;
using Leguar.TotalJSON;


public class Autorize : MonoBehaviour
{
    void Start(){
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            StartCoroutine(Send.Request("check_connection?param=" + success.ToString(), new JSON().CreateString(), AfterRegister));
        });
    }
    void AfterRegister(string response){

    }
}
