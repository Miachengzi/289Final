using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second : Father
{
    public string description;

    public override void RandomNum()
    {
        Debug.Log("Random Number " + Random.Range(0, 100));
    }
}
