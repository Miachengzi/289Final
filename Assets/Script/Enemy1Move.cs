using UnityEngine;

public class Enemy1Move : MonoBehaviour
{
    public Vector2 pointA;           // 输入坐标点A
    public Vector2 pointB;           // 输入坐标点B
    public float moveSpeed = 2f;     // 巡逻速度

    private Vector2 targetPos;

    void Start()
    {
        targetPos = pointB;
    }

    void Update()
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        Debug.DrawRay(transform.position, direction * 0.6f, Color.red);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 0.6f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("box"))
            {
                targetPos = (targetPos == pointA) ? pointB : pointA;
                return;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            targetPos = (targetPos == pointA) ? pointB : pointA;
        }
    }
}


