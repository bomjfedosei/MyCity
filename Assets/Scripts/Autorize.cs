using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Tools;


public class Autorize : MonoBehaviour
{
    void Start(){
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            
        });
    }
}
