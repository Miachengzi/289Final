using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Father 
{
    public string heroName;
    public int hp;
    public int mp;

    public void Introduction()
    {
        Debug.Log("My Name is " + heroName);
    }

    public abstract void RandomNum();
}
