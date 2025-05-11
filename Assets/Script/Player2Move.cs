using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player2Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Tilemap movableTilemap;
    public GameObject otherPlayer;

    private bool isMoving = false;

    private Vector3Int? targetCellInt = null;

    void Update()
    {
        if (!isMoving)
        {
            Vector3Int direction = Vector3Int.zero;

            if (Input.GetKey(KeyCode.UpArrow)) direction = Vector3Int.up;
            if (Input.GetKey(KeyCode.DownArrow)) direction = Vector3Int.down;
            if (Input.GetKey(KeyCode.LeftArrow)) direction = Vector3Int.left;
            if (Input.GetKey(KeyCode.RightArrow)) direction = Vector3Int.right;

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

        if (otherPlayer != null)
        {
            Vector3Int otherCell = movableTilemap.WorldToCell(otherPlayer.transform.position);
            if (targetCell == otherCell)
            {
                return; // 对方当前在这个格子
            }

            // 检测对方是否也要过来
            var otherMoveScript = otherPlayer.GetComponent<Plyer1Move>(); // 或 Player2Move 视情况替换
            if (otherMoveScript != null && otherMoveScript.IsMovingTo(targetCell))
            {
                return; // 对方正在前往这个格子
            }
        }


        Vector3 targetWorldPos = movableTilemap.CellToWorld(targetCell) + movableTilemap.cellSize / 2;
        Collider2D hitCollider = Physics2D.OverlapCircle(targetWorldPos, 0.1f);
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("box") || hitCollider.CompareTag("door"))
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

    IEnumerator MoveToPosition(Vector3 targetPos)
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
