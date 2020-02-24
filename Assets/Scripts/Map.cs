using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Leguar.TotalJSON;
using System.Linq;

public class Map : MonoBehaviour
{
    private int width, height;
    private Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();
    private Dictionary<Vector2Int, GameObject> Tiles = new Dictionary<Vector2Int, GameObject>();

    public int multiple;
    public GameObject Tile;
    public GameObject House, Woodcutter, Stone, Tree;

    public void SetParams()
    { 
        Vector2 size = GetComponent<Camera>()
            .ViewportToWorldPoint(new Vector2(0f, 0f)) - GetComponent<Camera>()
            .ViewportToWorldPoint(new Vector2(1f, 1f));
        this.width = (Math.Abs((int)size.x) + 1) * multiple;
        this.height = (Math.Abs((int)size.y) + 1) * multiple;
    }

    public string GetMapParams(int x, int y)
    {
        JSON coors = new JSON();
        coors.Add("x", x);
        coors.Add("y", y);
        JSON scope = new JSON();
        scope.Add("width", width);
        scope.Add("height", height);
        JSON result = new JSON();
        result.Add("coors", coors);
        result.Add("scope", scope);
        return result.CreateString();
    }

    private GameObject GetObject(string name)
    {
        switch (name)
        {
            case "spawn":
                return House;
            case "wood_cutter":
                return Woodcutter;
            case "tree":
                return Tree;
            case "rock":
                return Stone;
        }
        return new GameObject();
    }

    private void SetBorders(int x, int y)
    {
        CameraController camCon = GetComponent<CameraController>();
        camCon.borderXUp = x - width / 4;
        camCon.borderXDown = (x + width / 4) + 1;
        camCon.borderYUp = y - height / 4;
        camCon.borderYDown = (y + height / 4) + 1;
    }

    public void GenMap(int x, int y)
    {
        SetBorders(x, y);
        List<Vector2Int> MapCoors = new List<Vector2Int>();
        for (int i = (x - width / 2); i < (x + width / 2) + 1; i++)
        {
            for (int j = (y - height / 2); j < (y + height / 2) + 1; j++)
            {
                Vector2Int newTileCoors = new Vector2Int(i, j);
                if (!Tiles.ContainsKey(newTileCoors))
                {
                    GameObject createdTile = Instantiate(Tile, (Vector2)newTileCoors, Quaternion.identity);
                    Tiles.Add(newTileCoors, createdTile);
                }
                MapCoors.Add(newTileCoors);
            }
        }
        foreach (Vector2Int key in Tiles.Keys.ToList())
        {
            if (!MapCoors.Contains(key))
            {
                GameObject Value = new GameObject();
                Tiles.TryGetValue(key, out Value);
                Destroy(Value);
                Tiles.Remove(key);
            }
        }
        StartCoroutine(Send.Request("get_map", GetMapParams(x, y), DrawGenMap));
    }

    void DrawGenMap(string response)
    {
        List<string> createdObjectsKeys = new List<string>();
        JSON responseJSON = JSON.ParseString(response);
        if (!responseJSON.IsJNull("game_objects"))
        {
            JArray elementsArray = responseJSON.GetJArray("game_objects");
            for (int i = 0; i < elementsArray.Length; i++)
            {
                JSON element = elementsArray.GetJSON(i);
                string uuid = element.GetString("uuid");
                if (!Objects.ContainsKey(uuid))
                {
                    Vector2 coors = Format.createCoorsfromJSON(element.GetJSON("coors"));
                    GameObject createdObject = Instantiate(
                        GetObject(element.GetString("name")),
                        coors, Quaternion.identity);
                    Objects.Add(uuid, createdObject);
                }
                createdObjectsKeys.Add(uuid);
            }
        }
        foreach (string key in Objects.Keys.ToList())
        {
            if (!createdObjectsKeys.Contains(key))
            {
                GameObject Value = new GameObject();
                Objects.TryGetValue(key, out Value);
                Destroy(Value);
                Objects.Remove(key);
            }
        }
    }

    private void Start()
    {
        SetParams();
        GenMap(0, 0);
    }

}
