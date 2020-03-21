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
            if (success){
                JSON body = new JSON();
                body.Add("user_id", Social.localUser.id);
                body.Add("username", Social.localUser.userName); 
                StartCoroutine(Send.Request("register_user", body.CreateString(), AfterRegister));
            }
        });
    }
    void AfterRegister(string response){

    }
}
