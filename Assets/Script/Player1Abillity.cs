using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player1Abillity : MonoBehaviour
{
    public Tilemap movableTilemap;
    private AudioSource pushSound;

    private float originalSpeed;
    public float slowSpeed = 2f;

    void Start()
    {
        pushSound = GetComponent<AudioSource>(); // 获取玩家身上的音频组件
    }

    public bool TryPushBox(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 boxPos = currentPos + direction;
        Vector2 boxTargetPos = currentPos + direction * 2;

        Collider2D hitBox = Physics2D.OverlapCircle(boxPos, 0.1f);

        if (hitBox != null && hitBox.CompareTag("box"))
        {
            Vector3Int boxTargetCell = movableTilemap.WorldToCell(boxTargetPos);
            if (!movableTilemap.HasTile(boxTargetCell))
            {
                return false;
            }

            Collider2D hitObstacle = Physics2D.OverlapCircle(boxTargetPos, 0.1f);
            if (hitObstacle == null)
            {
                Plyer1Move moveScript = GetComponent<Plyer1Move>();
                originalSpeed = moveScript.moveSpeed;
                moveScript.moveSpeed = slowSpeed;

                if (pushSound != null) pushSound.Play();

                StartCoroutine(MoveBoxSmoothly(hitBox.transform, boxTargetPos, slowSpeed));
                StartCoroutine(RecoverSpeedAfterDelay(0.5f, moveScript)); // 自动恢复原速

                return true;
            }
            else
            {
                if (hitObstacle.CompareTag("enemy") || hitObstacle.CompareTag("enemybaby") || hitObstacle.CompareTag("door") || hitObstacle.CompareTag("Player") || hitObstacle.CompareTag("box") || hitObstacle.CompareTag("fruit") || hitObstacle.CompareTag("gem"))
                {
                    return false;
                }

                return false;
            }
        }

        return true;
    }

    public bool TryPullBox(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 boxPos = currentPos - direction;
        Vector2 playerTargetPos = currentPos + direction;

        Collider2D hitBox = Physics2D.OverlapCircle(boxPos, 0.1f);
        if (hitBox == null || !hitBox.CompareTag("box"))
        {
            return false;
        }

        Vector3Int frontCell = movableTilemap.WorldToCell(playerTargetPos);
        if (!movableTilemap.HasTile(frontCell))
        {
            return false;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(playerTargetPos, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("door") || hit.CompareTag("fruit") || hit.CompareTag("gem"))
            {
                return false;
            }
        }

        Plyer1Move moveScript = GetComponent<Plyer1Move>();
        originalSpeed = moveScript.moveSpeed;
        moveScript.moveSpeed = slowSpeed;

        if (pushSound != null) pushSound.Play();

        StartCoroutine(MoveBoxSmoothly(hitBox.transform, currentPos, slowSpeed));
        StartCoroutine(GetComponent<Plyer1Move>().MoveToPosition(playerTargetPos));
        StartCoroutine(RecoverSpeedAfterDelay(0.5f, moveScript));
        return true;
    }

    private IEnumerator MoveBoxSmoothly(Transform box, Vector2 targetPos, float speed)
    {
        while (Vector2.Distance(box.position, targetPos) > 0.01f)
        {
            box.position = Vector2.MoveTowards(box.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        box.position = targetPos;
    }

    private IEnumerator RecoverSpeedAfterDelay(float delay, Plyer1Move moveScript)
    {
        yield return new WaitForSeconds(delay);
        if (moveScript != null)
        {
            moveScript.moveSpeed = originalSpeed;
        }
    }
}

