using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cursor : MonoBehaviour
{
    private Vector3 cursorPos;

    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, transform.position.z);
    }
}
