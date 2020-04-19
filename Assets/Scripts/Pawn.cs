using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Leguar.TotalJSON;


public class Pawn : MonoBehaviour
{
    void OnMouseDown(){
        
    }

    void ActionsView(string response){
        Debug.Log(response);
    }
    
    public void DrawAction(JSON action)
    {
        if (action.ContainsKey("way"))
        {
            DrawPath(action.GetJArray("way"));
        }
    }

    void DrawPath(JArray way)
    {
        GetComponent<LineRenderer>().positionCount = way.Length;
        Vector3[] points = new Vector3[way.Length];
        for (int i = 0; i < way.Length; i++)
        {
            JArray pointJArray = way.GetJArray(i);
            points[i] = new Vector3(pointJArray.GetInt(0), pointJArray.GetInt(1), 0);
        }
        GetComponent<LineRenderer>().SetPositions(points);
    }
}
