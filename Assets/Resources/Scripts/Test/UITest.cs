using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public RectTransform UICanvas;
    public Camera camera;
    Vector2 localPoint;

    public GameObject effect;
    public Transform parent;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas, Input.mousePosition, camera, out localPoint))
            {
                GameObject newEffect = Instantiate(effect, parent);
                newEffect.AddComponent<RectTransform>().anchoredPosition = localPoint;
                newEffect.GetComponent<RectTransform>().localScale *= 100;
                Destroy(newEffect, 1.5f);
            }
        }
    }
}
