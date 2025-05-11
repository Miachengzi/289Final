using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plyer1Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Tilemap movableTilemap;
    public GameObject otherPlayer;

    private bool isMoving = false;

    private Vector3Int? targetCellInt = null;

    public bool IsMoving()
    {
        return isMoving;
    }

    void Update()
    {
        // 1. 新加的部分：检测是不是在格子中心
        Vector3 pos = transform.position;
        float xFraction = pos.x - Mathf.Floor(pos.x); // 取x的小数部分
        float yFraction = pos.y - Mathf.Floor(pos.y); // 取y的小数部分

        bool isOnCenter = Mathf.Abs(xFraction - 0.5f) < 0.05f && Mathf.Abs(yFraction - 0.5f) < 0.05f;

        // 2. 你原来的逻辑，保持不变
        if (!isMoving)
        {
            Vector3Int direction = Vector3Int.zero;

            if (Input.GetKey(KeyCode.W)) direction = Vector3Int.up;
            if (Input.GetKey(KeyCode.S)) direction = Vector3Int.down;
            if (Input.GetKey(KeyCode.A)) direction = Vector3Int.left;
            if (Input.GetKey(KeyCode.D)) direction = Vector3Int.right;

            if (direction != Vector3Int.zero)
            {
                TryMove(direction);
            }
        }
    }

    void TryMove(Vector3Int dir)
    {
        Vector3Int currentCell = movableTilemap.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + dir;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Player1Abillity ability = GetComponent<Player1Abillity>();
            if (ability != null)
            {
                Vector2 pullDir = new Vector2(dir.x, dir.y);
                bool pulled = ability.TryPullBox(pullDir);
                if (pulled)
                {
                    return;
                }
            }
        }



        if (otherPlayer != null)
        {
            Vector3Int otherCell = movableTilemap.WorldToCell(otherPlayer.transform.position);
            if (targetCell == otherCell)
            {
                return; // 对方当前在这个格子
            }

            // 检测对方是否也要过来
            var otherMoveScript = otherPlayer.GetComponent<Player2Move>(); // 或 Player2Move 视情况替换
            if (otherMoveScript != null && otherMoveScript.IsMovingTo(targetCell))
            {
                return; // 对方正在前往这个格子
            }
        }

        Vector3 targetWorldPos = movableTilemap.CellToWorld(targetCell) + movableTilemap.cellSize / 2;
        Collider2D hitCollider = Physics2D.OverlapCircle(targetWorldPos, 0.1f);




        if (hitCollider != null && hitCollider.CompareTag("box"))
        {
            // 👉 如果正在按着 Shift（用于拉箱子），则不允许推箱子
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }

            Player1Abillity ability = GetComponent<Player1Abillity>();
            if (ability != null)
            {
                Vector2 moveDir = new Vector2(dir.x, dir.y);
                bool pushed = ability.TryPushBox(moveDir);
                if (!pushed)
                {
                    return;
                }
            }
        }

        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("door"))
            {
                return;
            }
        }

        if (movableTilemap.HasTile(targetCell))
        {
            Vector3 worldPos = movableTilemap.CellToWorld(targetCell) + movableTilemap.cellSize / 2;

            // 防止移动出屏幕
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(worldPos);
            if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
            {
                return; // 出界
            }
            targetCellInt = targetCell;

            StartCoroutine(MoveToPosition(worldPos));
        }
    }

    public IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        targetCellInt = null;
        isMoving = false;
    }

    public bool IsMovingTo(Vector3Int cell)
    {
        return targetCellInt.HasValue && targetCellInt.Value == cell;
    }
}