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
    void Start(){
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.OpenWithAutomaticConflictResolution("MyToken.sav", DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
            }
            else
            {

            }
        }
        );
    }

    void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Send.Request("check_connection?status=" + game.IsOpen, new JSON().CreateString(), Output);
        }
    }

    void Output(string ans)
    {

    }
}
