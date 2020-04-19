using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Leguar.TotalJSON;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

namespace Tools{
    public class Send{
        public static Dictionary<string, UnityWebRequest> items = new Dictionary<string, UnityWebRequest>();
        private static string rootUrl = "https://bomjfedosei.fvds.ru/async/";
        public static IEnumerator Request(string method, string json, Action<string> CallBack){
            Debug.Log("SENDED : " + json);
            string url = rootUrl + method;
            var uwr = new UnityWebRequest(url, "POST");
            items.Add(method, uwr);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();
            if (uwr.error != null){
                Debug.Log(uwr.error);
            }
            else{
                Debug.Log("RECEIVED :" + uwr.downloadHandler.text);
                CallBack(uwr.downloadHandler.text);
            }
        }
    }
    public static class Format
    {
        public static Vector2 createCoorsfromJSON(JSON coors)
        {
            int x = coors.GetInt("x");
            int y = coors.GetInt("y");
            return new Vector2(x, y);
        }
    }

    public static class Connections
    {
        private static string token = "";
        public static string getToken()
        {
            return token;
        }

        public static void setToken(string newToken)
        {
            token = newToken;
        }
    }
}
