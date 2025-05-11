using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float maxDist;
    public LayerMask mask;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        SelfRotation();
        Detect();
    }

    private void SelfRotation()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        //Debug.DrawLine(transform.position, transform.up * 10, Color.red);//MARKER 这里先写成之前的【transform.forward】看看效果
    }

    private void Detect()
    {
        //Version 1 LayerMask
        //Ray ray = new Ray(transform.position, transform.up);//MARKER 这里不能用Ray是因为Ray包含的是Vector3结构体，这是2D游戏
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, maxDist, LayerMask.GetMask("Block"));

        if (hitInfo.collider != null)
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            //Debug.DrawLine(transform.position, transform.up * 15, Color.red);//MARKER 如果想要射线停止
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.up * 15, Color.green);
        }

        //MARKER LAYERMAKER / mask.value / 2 to the power n / LayerMask.GetMask("Block") / ~LayerMask.GetMask("Block") /  
        //MARKER 1 << LayerMask.NameToLayer("Block")

        //Version 2 Tag
        //RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, maxDist);

        //if (hitInfo.collider != null)
        //{

        //    if(hitInfo.collider.CompareTag("Block"))
        //    {
        //        Debug.Log(hitInfo.collider.gameObject.name);
        //        Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        //    }
        //    else
        //    {
        //        Debug.DrawLine(transform.position, transform.up * 15, Color.green);
        //    }
        //}
        //else
        //{
        //    Debug.DrawLine(transform.position, transform.up * 15, Color.green);
        //}
    }
}
