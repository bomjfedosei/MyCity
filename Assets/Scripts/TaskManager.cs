using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;
using Leguar.TotalJSON;
using System;
using System.Linq;

public class TaskManager : MonoBehaviour
{
    public GameObject[] Tasks;
    public GameObject[] SetTasks;
    public Sprite Axe, Plus;
    public Text Estimate, CommonTime;
    private int TaskId;
    private string Type;
    string[] Actions;
    private string UUID, TaskUUID;


    public void Show(string uuid, string type)
    {
        Type = type;
        UUID = uuid;
        JSON parameters = new JSON();
        parameters.Add("GP_ID", "g05987395182658537218");
        parameters.Add("pawn_uuid", uuid);
        StartCoroutine(Send.Request("get_pawn_tasks_list", parameters.CreateString(), ShowTasks));
    }

    void ShowTasks(string responseJSON)
    {
        JSON response = JSON.ParseString(responseJSON);
        if (response.GetBool("status"))
        {
            JArray tasks = response.GetJArray("tasks");
            bool chosen = false;
            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks.IsJNull(i))
                {
                    Tasks[i].SetActive(true);
                    JSON task = tasks.GetJSON(i);
                    if (task.GetString("name") == "cut")
                    {
                        Tasks[i].GetComponent<Image>().sprite = Axe;
                    }
                    if (i == 0)
                    {
                        Estimate.GetComponent<TimeTextEstimate>().SetEstimatedTime(task.GetInt("end_time"));
                    }
                }
                else if (tasks.IsJNull(i))
                {
                    if (!chosen)
                    {
                        chosen = true;
                        TaskId = i;
                        Tasks[i].SetActive(true);
                        Tasks[i].GetComponent<Image>().sprite = Plus;
                        Tasks[i].GetComponent<Button>().onClick.AddListener(AddTaskToPawn);
                    }   
                    else
                    {
                        Tasks[i].SetActive(false);
                    }
                }
            }
        }
    }



    void AddTaskToPawn()
    {
        foreach (GameObject SetTask in SetTasks)
        {
            SetTask.SetActive(true);
        }
        JSON parameters = new JSON();
        parameters.Add("GP_ID", "g05987395182658537218");
        parameters.Add("task_name", "cut");
        parameters.Add("object_uuid", UUID);
        StartCoroutine(Send.Request("add_task_to_pawn", parameters.CreateString(), TaskToPawn));
    }

    void TaskToPawn(string responseJSON)
    {
        JSON response = JSON.ParseString(responseJSON);
        TaskUUID = response.GetString("task_uuid");
        CommonTime.GetComponent<Text>().text = TimeSpan.FromSeconds(response.GetInt("common_time")).ToString();
        SetTasks[0].GetComponent<Button>().onClick.AddListener(Accept);
        SetTasks[1].GetComponent<Button>().onClick.AddListener(Decine);
    }

    void Accept()
    {
        AcceptTask(true);
    }

    void Decine()
    {
        AcceptTask(false);
    }

    void AcceptTask(bool answer)
    {
        foreach (GameObject SetTask in SetTasks)
        {
            SetTask.SetActive(false);
        }
        foreach(GameObject Task in Tasks)
        {
            Task.SetActive(false);
        }
        GameObject camera = GameObject.Find("Camera");
        camera.GetComponent<CameraController>().SetUnLead();
        JSON parameters = new JSON();
        parameters.Add("accept", answer);
        parameters.Add("GP_ID", "g05987395182658537218");
        parameters.Add("task_uuid", TaskUUID);
        StartCoroutine(Send.Request("accept_task", parameters.CreateString(), AcceptTaskResponse));
    }

    void AcceptTaskResponse(string responseJSON)
    {
        JSON response = JSON.ParseString(responseJSON);
        if (response.Keys.Contains("task_uuid"))
        {
            GameObject pawn = GameObject.Find(UUID);
            pawn.GetComponent<Pawn>().GetNextAction();
        }
    }
}
