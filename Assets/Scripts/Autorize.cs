using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Tools;


public class Autorize : MonoBehaviour
{
    void Awake(){
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .RequestIdToken()
        .RequestServerAuthCode(true)
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }

    void Start(){
        Social.localUser.Authenticate((bool success) => {
            Debug.Log(success);
        });
    }
}
