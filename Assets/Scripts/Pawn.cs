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
    float[] WayDistanceCuts;
    int Orientation; // 0 - Right | 1 - Left | 2 - Up | 3 - Down
    Animator Anim;
    bool isGettingNextAction;

    private void Start()
    {
        isGettingNextAction = false;
        Anim = GetComponentInChildren<Animator>();
    }

    private void OnMouseDown()
    {
        GameObject Camera = GameObject.Find("Camera");
        Camera.GetComponent<CameraController>().SetLead(gameObject);
        Camera.GetComponent<TaskManager>().Show(
            GetComponent<Element>().getKey(),
            GetComponent<Element>().type);
    }

    private void Update()
    {
        if (Action != null){
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            if (EndTime > epoch)
            {
                if (isMove())
                {
                    transform.position = GetCurrentPos() + new Vector3(0f, GetComponent<Element>().marginY, 0f);
                }
                else
                {
                    LineRenderer lineRenderer = GetComponent<LineRenderer>();
                    transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1) + new Vector3(0f, GetComponent<Element>().marginY, 0f);
                }
            }
            else
            {
                GetNextAction();
            }
        }
        SetAnimation();
    } 

    public void DrawAction(JSON action)
    {
        if (action.ContainsKey("way"))
        {
            Way = GetWay(action.GetJArray("way"));
            Action = action.GetString("action_name");
            StartTime = action.GetInt("start_time");
            EndTime = action.GetInt("end_time");
            WayDistanceCuts = CountDistanceCuts(Way);
            EstimatedTime = TimeSpan.FromSeconds(EndTime - StartTime);
            Speed = (double)WayDistanceCuts.Sum() / EstimatedTime.TotalMilliseconds;
            GetComponent<LineRenderer>().positionCount = Way.Length;
            GetComponent<LineRenderer>().SetPositions(Way);
            if (!isForward())
            {
                Array.Reverse(WayDistanceCuts);
                Array.Reverse(Way);
            }
            if (!isMove())
            {
                GameObject target = GameObject.Find(action.GetString("target_uuid"));
                if (target != null)
                {
                    float marginY = target.GetComponent<Element>().marginY;
                    Vector3 targetCoors = target.transform.position - new Vector3(0f, marginY, 0f);
                    Debug.Log(targetCoors);
                    Orientation = GetOrientation(targetCoors - Way[Way.Length - 1]);
                }
            }
        }
    }

    public void GetNextAction()
    {
        if (isGettingNextAction == false)
        {
            isGettingNextAction = true;
            JSON parameters = new JSON();
            //parameters.Add("GP_ID", "g05987395182658537218");
            parameters.Add("GP_ID", PlayerPrefs.GetString("gp_id"));
            parameters.Add("object_uuid", GetComponent<Element>().getKey());
            StartCoroutine(Send.Request("get_current_action", parameters.CreateString(), NextAction));
        }
    }

    void NextAction(string responseJSON)
    {
        JSON response = JSON.ParseString(responseJSON);
        if (response.GetBool("status"))
        {
            if (!response.IsJNull("action"))
            {
                DrawAction(response.GetJSON("action"));
            }
            else
            {
                Action = null;
            }
        }
        isGettingNextAction = false;
    }

    private void SetAnimation()
    {
        Anim.SetBool("isWalk", isMove());
        Anim.SetInteger("Orientation", Orientation);
        Anim.SetBool("isCarry", isCarry());
        Anim.SetBool("isCut", isCut());

    }

    public void DropAction()
    {
        Action = null;
        //Anim.SetTrigger("isIdle");
    }

    Vector3 GetCurrentPos()
    {
        long epoch = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Vector3[] localWay = Way;
        TimeSpan StartTimeTS = TimeSpan.FromSeconds(StartTime);
        double StartTimeMS = StartTimeTS.TotalMilliseconds;
        double covDistance = Speed * (epoch - StartTimeMS);
        for (int i = 0; i < WayDistanceCuts.Length; i++)
        {
            float cut = WayDistanceCuts[i];
            if (covDistance > cut)
            {
                covDistance -= cut;
            }
            else
            {
                Orientation = GetOrientation(localWay[i + 1] - localWay[i]);
                Vector3 result = localWay[i] + (localWay[i + 1] - localWay[i]) * (float)(covDistance / WayDistanceCuts[i]);
                return result;
            }
        }
        return new Vector3();
    }

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

    int GetOrientation(Vector3 Inc)
    {
        if (Inc.x > 0f)
        {
            return 0;
        }
        else if (Inc.x < 0f)
        {
            return 1;
        }
        else if (Inc.y > 0f)
        {
            return 2;
        }
        else if (Inc.y < 0f)
        {
            return 3;
        }
        return 0;
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

    bool isCarry()
    {
        string[] carry = { "carry" };
        return carry.Contains(Action);
    }

    bool isCut()
    {
        string[] cut = { "cut" };
        return cut.Contains(Action);
    }

}
