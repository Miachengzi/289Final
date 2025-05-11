using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHero : Second
{
    public override void RandomNum()
    {
        Debug.Log("Random Number " + Random.Range(0, 5));
    }
}
