using UnityEngine;

public class Element : MonoBehaviour
{
    public float marginY;
    public string type;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + marginY);
    }  
}
