using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Leguar.TotalJSON;


namespace Tools{
    public class Send{
        private static string rootUrl = "https://bomjfedosei.fvds.ru/async/";
        public static IEnumerator Request(string method, string json, Action<string> CallBack){
            string url = rootUrl + method;
            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();
            if (uwr.error != null){
                Debug.Log(uwr.error);
            }
            else{
                Debug.Log(uwr.downloadHandler.text);
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
}
