using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToHome : MonoBehaviour
{
    public GameObject Camera;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Teleport);
    }

    void Teleport()
    {
        Vector3 coors = new Vector3(65, -147, -10);
        Camera.transform.position = coors;
        Camera.GetComponent<Map>().GenMap(65, -147);
    }
}
