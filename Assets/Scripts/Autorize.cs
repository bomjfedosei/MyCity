using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Tools;
using Leguar.TotalJSON;
using GooglePlayGames.BasicApi.SavedGame;

public class Autorize : MonoBehaviour
{
    public Text label;
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((success) => { 
            if (success)
            {
                JSON userParams = new JSON();
                userParams.Add("GP_ID", Social.localUser.id);
                userParams.Add("username", Social.localUser.userName);
                Send.Request("register_user", userParams.CreateString(), RegisterCallBack);
            }
        });
    }

    void RegisterCallBack(string response)
    {
        JSON responseJSON = JSON.ParseString(response);
        label.text = Social.localUser.id;
    }
}
