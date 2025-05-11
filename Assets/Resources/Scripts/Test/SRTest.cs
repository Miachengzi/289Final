using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
}
