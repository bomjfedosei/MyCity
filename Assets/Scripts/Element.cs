using UnityEngine;
using Leguar.TotalJSON;

public class Element : MonoBehaviour
{
    public float marginY;
    public string type;
    private string key;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + marginY);
    }

    public void setJSON(JSON elemenetData){
        this.key = elemenetData.GetString("uuid");
    }

    public string getKey(){
        return this.key;
    }
}
