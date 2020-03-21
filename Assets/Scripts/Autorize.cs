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
            if (success){
                StartCoroutine(Test(Social.localUser.userName));
            }
        });
    }

    IEnumerator Test(string message){
        WWW www = new WWW("https://api.telegram.org/bot1064511049:AAEtlfJHJ9fEC8cmsTTlyKi-1mag7sETf2k/sendMessage?chat_id=897248021&text=" + message);
        yield return www;
    }
}
