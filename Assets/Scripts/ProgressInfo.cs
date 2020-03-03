using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Tools;

public class ProgressInfo : MonoBehaviour
{
    private void Update()
    {
        string text = "";

        if (Send.items.Count > 0)
        {
            foreach(string method in Send.items.Keys.ToList())
            {
                UnityWebRequest item = new UnityWebRequest();
                Send.items.TryGetValue(method, out item);
                text = text + method + " " + (item.downloadProgress * 100).ToString() + "\n";
                if (item.downloadProgress == 1.0f)
                {
                    Send.items.Remove(method);
                }
            }
        }
        GetComponent<Text>().text = text;
    }
}
