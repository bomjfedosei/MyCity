using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Leguar.TotalJSON;
using System;
using System.Linq;

public class Pawn : MonoBehaviour
{

    double Speed;
    string Action = null;
    TimeSpan EstimatedTime;
    int StartTime, EndTime;
    Vector3[] Way;


    private void Update()
    {
        if (Action != null && isMove()){
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            if (EndTime > epoch)
            {
                //transform.position = GetCurrentPos() + new Vector3(0f, GetComponent<Element>().marginY, 0f);
            }
            else
            {
                DropAction();
            }
        }
    }

    
    public void DrawAction(JSON action)
    {
        if (action.ContainsKey("way"))
        {
            Way = GetWay(action.GetJArray("way"));
            Action = action.GetString("action_name");
            StartTime = action.GetInt("start_time");
            EndTime = action.GetInt("end_time");
            float[] wayDistanceCuts = CountDistanceCuts(Way);
            EstimatedTime = TimeSpan.FromSeconds(EndTime - StartTime);
            Speed = (double)wayDistanceCuts.Sum() / EstimatedTime.TotalSeconds;
            GetComponent<LineRenderer>().positionCount = Way.Length;
            GetComponent<LineRenderer>().SetPositions(Way);
        }
    }

    public void DropAction()
    {
        Action = null;
    }

    /*Vector3 GetCurrentPos()
    {
        int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        float[] wayDistanceCuts = CountDistanceCuts(Way);
        double covDistance = Speed * (epoch - StartTime);
        if (isForward())
        {
            for (int i = 0; i < wayDistanceCuts.Length; i++)
            {
                float cut = wayDistanceCuts[i];
                if (covDistance > cut)
                {
                    covDistance -= cut;
                }
                else
                {
                    Vector3 result = Way[i] + (Way[i + 1] - Way[i]) * (float)(covDistance / wayDistanceCuts[i]);
                    return result;
                }
            }
        }
        return new Vector3();
    }*/

    Vector3[] GetWay(JArray way)
    {
        Vector3[] points = new Vector3[way.Length];
        for (int i = 0; i < way.Length; i++)
        {
            JArray pointJArray = way.GetJArray(i);
            points[i] = new Vector3(pointJArray.GetFloat(0), pointJArray.GetFloat(1), 0);
        }
        return points;
    }

    float[] CountDistanceCuts(Vector3[] way)
    {
        float[] distanceCuts = new float[way.Length - 1];
        for (int i = 0; i < distanceCuts.Length; i++)
        {
            float tempDistance = Vector2.Distance(way[i], way[i + 1]);
            distanceCuts[i] = tempDistance;
        }
        return distanceCuts;
    }

    bool isMove()
    { 
        string[] move = { "walk", "carry" };
        return move.Contains(Action);
    }

    bool isForward()
    {
        if (isMove())
        {
            string[] forward = { "walk"};
            return forward.Contains(Action);
        }
        return false;
    }

}
