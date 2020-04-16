using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        Social.localUser.Authenticate((success, message) => { 
            if (success)
            {
                JSON userParams = new JSON();
                userParams.Add("GP_ID", Social.localUser.id);
                userParams.Add("username", Social.localUser.userName);
                StartCoroutine(Send.Request("register_user", userParams.CreateString(), RegisterCallBack));
            }
            else
            {
                label.text = message;
            }
        });
    }

    void RegisterCallBack(string response)
    {
        JSON responseJSON = JSON.ParseString(response);
        bool isNewUser = responseJSON.GetBool("is_new_user");
        GetData();
    }


    void GetData()
    {
        PlayerPrefs.SetString("gp_id", Social.localUser.id);
        JSON userParams = new JSON();
        userParams.Add("GP_ID", Social.localUser.id);
        StartCoroutine(Send.Request("get_profile", userParams.CreateString(), GetDataCallback));
    }

    void GetDataCallback(string response)
    {
        label.text = response;
        JSON responseJSON = JSON.ParseString(response);
        PlayerPrefs.SetString("username", responseJSON.GetString("username"));
        PlayerPrefs.SetInt("spawn_x", responseJSON.GetJSON("spawn").GetInt("x"));
        PlayerPrefs.SetInt("spawn_y", responseJSON.GetJSON("spawn").GetInt("y"));
        SceneManager.LoadScene(1);
        SceneManager.UnloadSceneAsync(0); 
    }
}
