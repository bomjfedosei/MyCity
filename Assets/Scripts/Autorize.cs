using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Tools;
using Leguar.TotalJSON;
using GooglePlayGames.BasicApi.SavedGame;

public class Autorize : MonoBehaviour
{
    private void Start()
    {
        GPGSManager.Initialize(false);
        GPGSManager.Auth((success) =>
        {
            if (success && !PlayerPrefs.HasKey("token"))
            {
                GPGSManager.ReadSaveData(GPGSManager.DEFAULT_SAVE_NAME, (status, data) =>
                {
                    if (status == SavedGameRequestStatus.Success && data.Length > 0)
                    {
                        
                    }
                    else
                    {
                        JSON registerData = new JSON();
                        registerData.Add("user_id", GPGSManager.GetUserId());
                        registerData.Add("username", GPGSManager.GetUsername());
                        Send.Request("register_user", registerData.CreateString(), RegisterUser);
                    }
                    
                });
            }
        });
    }

    void RegisterUser(string response)
    {

    }
}
