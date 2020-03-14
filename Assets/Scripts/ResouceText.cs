using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResouceText : MonoBehaviour
{
    public GameObject Sprite;
    public Sprite Wood;
    public Text Count;

    public IEnumerator showSprite(string resource, int count){
        Sprite.GetComponent<SpriteRenderer>().sprite = GetSprite(resource);
        Sprite.GetComponent<SpriteRenderer>().enabled = true;
        Count.text = count.ToString();
        yield return new WaitForSeconds(3);
        Sprite.GetComponent<SpriteRenderer>().enabled = false;
        Count.text = "";
        yield return null;
    }

    private Sprite GetSprite(string name){
        if (name == "wood"){
            return Wood;
        }
        return null;
    }
}
