using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToHome : MonoBehaviour
{
    public GameObject Camera;
    public int x, y;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Teleport);
        x = PlayerPrefs.GetInt("spawn_x");
        y = PlayerPrefs.GetInt("spawn_y");
    }

    void Teleport()
    {
        Vector3 coors = new Vector3(x, y, -10);
        Camera.transform.position = coors;
        Camera.GetComponent<Map>().GenMap(x, y);
    }
}
