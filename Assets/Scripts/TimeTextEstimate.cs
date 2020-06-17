using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeTextEstimate : MonoBehaviour
{
    private int EndTime;


    private void Start()
    {
        //EndTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    void Update()
    {
        int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        if (EndTime > epoch)
        {
            TimeSpan offset = TimeSpan.FromSeconds(EndTime - epoch);
            GetComponent<Text>().text = offset.ToString();
        }
    }


    public void SetEstimatedTime(int endTime)
    {
        EndTime = endTime;
    }
}
