using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy3Move : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float detectionTime = 2f;
    public float moveSpeed = 5f;

    public Tilemap tilemap;
    public GameObject player1;
    public GameObject player2;

    private GameObject targetPlayer = null;
    private float timer = 0f;
    private bool isChasing = false;

    public GameObject warningUI;

    void Start()
    {
        if (warningUI != null)
            warningUI.SetActive(false);
    }
    
    void Update()
    {
        if (isChasing || player1 == null || player2 == null) return;

        float dist1 = Vector3Int.Distance(tilemap.WorldToCell(transform.position), tilemap.WorldToCell(player1.transform.position));
        float dist2 = Vector3Int.Distance(tilemap.WorldToCell(transform.position), tilemap.WorldToCell(player2.transform.position));

        if (dist1 <= detectionRadius || dist2 <= detectionRadius)
        {
            timer += Time.deltaTime;

            if (timer >= detectionTime)
            {
                // 选择最近玩家
                if (dist1 < dist2) targetPlayer = player1;
                else if (dist2 < dist1) targetPlayer = player2;
                else targetPlayer = Random.value < 0.5f ? player1 : player2;

                StartCoroutine(MoveToTarget(targetPlayer));
                isChasing = true;
            }
        }
        else
        {
            timer = 0f; // 离开范围则重置
        }
    }

    IEnumerator MoveToTarget(GameObject target)
    {
        Vector3Int targetCell = tilemap.WorldToCell(target.transform.position);
        Vector3 worldPos = tilemap.CellToWorld(targetCell) + tilemap.cellSize / 2;

        // 🔥 显示警告图标在目标位置，并立即隐藏（只闪现一下）
        if (warningUI != null)
        {
            warningUI.transform.position = worldPos;
            warningUI.SetActive(true);
            yield return new WaitForSeconds(0.3f);  // 保持短时间警告
            warningUI.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f); // UI闪现完后再移动

        // 🧭 正式开始移动
        while (Vector3.Distance(transform.position, worldPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = worldPos;
        isChasing = false;
        timer = 0f;
    }
}
