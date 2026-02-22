using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatObject : object
{
    
    
    public string name;
    public Dictionary<string, Sprite> SpriteDict { private set; get; }

    public AudioClip meow;

    public CatObject(string name)
    {
        this.name = name;
    }



}
