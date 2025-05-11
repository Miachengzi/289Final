using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushPullBox : MonoBehaviour
{
    public float normalMoveSpeed = 5f;
    public float pushPullMoveSpeed = 1.5f;
    public Tilemap movableTilemap;
    public AudioSource pushPullAudio;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isPulling = false;
    private GameObject grabbedBox = null;

    private readonly string[] forbiddenTags = { "enemy", "enemybaby", "door", "Player", "box", "fruit", "gem" };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Vector3Int direction = Vector3Int.zero;

        if (Input.GetKey(KeyCode.W)) direction = Vector3Int.up;
        if (Input.GetKey(KeyCode.S)) direction = Vector3Int.down;
        if (Input.GetKey(KeyCode.A)) direction = Vector3Int.left;
        if (Input.GetKey(KeyCode.D)) direction = Vector3Int.right;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (grabbedBox == null)
            {
                TryGrabBox();
            }
            isPulling = true;
        }
        else
        {
            if (grabbedBox != null)
            {
                grabbedBox = null;
                StopPushPullAudio();
            }
            isPulling = false;
        }

        if (direction != Vector3Int.zero)
        {
            if (isPulling && grabbedBox != null)
                TryPull(direction);
            else
                TryPush(direction);
        }
    }

    void TryGrabBox()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.6f);
        if (hit != null && hit.CompareTag("box"))
        {
            grabbedBox = hit.gameObject;
        }
    }

    void TryPush(Vector3Int dir)
    {
        Vector3Int currentCell = movableTilemap.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + dir;

        Vector3 targetWorldPos = movableTilemap.CellToWorld(targetCell) + movableTilemap.cellSize / 2;

        Collider2D hit = Physics2D.OverlapCircle(targetWorldPos, 0.1f);
        if (hit != null && hit.CompareTag("box"))
        {
            Vector3Int boxCell = movableTilemap.WorldToCell(hit.transform.position);
            Vector3Int boxTargetCell = boxCell + dir;

            if (CanMoveTo(boxTargetCell))
            {
                StartCoroutine(MovePlayerAndBox(hit.gameObject, dir));
            }
        }
        else
        {
            // ÆÕÍ¨ÒÆ¶¯
            if (CanMoveTo(targetCell))
            {
                StartCoroutine(MovePlayerOnly(dir));
            }
        }
    }

    void TryPull(Vector3Int dir)
    {
        if (grabbedBox == null) return;

        Vector3 playerPos = transform.position;
        Vector3 boxPos = grabbedBox.transform.position;

        Vector3 pullDir = (boxPos - playerPos).normalized;
        Vector3 inputDir = new Vector3(dir.x, dir.y, 0).normalized;

        if (Vector3.Dot(pullDir, inputDir) > 0.7f)
        {
            Vector3Int boxCell = movableTilemap.WorldToCell(grabbedBox.transform.position);
            Vector3Int boxTargetCell = boxCell + dir;

            if (CanMoveTo(boxTargetCell))
            {
                StartCoroutine(MovePlayerAndBox(grabbedBox, dir));
            }
        }
    }

    bool CanMoveTo(Vector3Int cell)
    {
        if (!movableTilemap.HasTile(cell))
            return false;

        Vector3 worldPos = movableTilemap.CellToWorld(cell) + movableTilemap.cellSize / 2;
        Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.1f);

        if (hit != null)
        {
            foreach (string tag in forbiddenTags)
            {
                if (hit.CompareTag(tag))
                    return false;
            }
        }

        return true;
    }

    IEnumerator MovePlayerAndBox(GameObject box, Vector3Int dir)
    {
        isMoving = true;

        float originalSpeed = normalMoveSpeed;
        float speed = pushPullMoveSpeed;

        Vector3 boxTargetPos = box.transform.position + new Vector3(dir.x, dir.y, 0);
        Vector3 playerTargetPos = transform.position + new Vector3(dir.x, dir.y, 0);

        PlayPushPullAudio();

        while (Vector3.Distance(box.transform.position, boxTargetPos) > 0.01f)
        {
            box.transform.position = Vector3.MoveTowards(box.transform.position, boxTargetPos, speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPos, speed * Time.deltaTime);
            yield return null;
        }

        box.transform.position = boxTargetPos;
        transform.position = playerTargetPos;

        StopPushPullAudio();

        isMoving = false;
    }

    IEnumerator MovePlayerOnly(Vector3Int dir)
    {
        isMoving = true;

        Vector3 playerTargetPos = transform.position + new Vector3(dir.x, dir.y, 0);

        while (Vector3.Distance(transform.position, playerTargetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPos, normalMoveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = playerTargetPos;

        isMoving = false;
    }

    void PlayPushPullAudio()
    {
        if (pushPullAudio != null && !pushPullAudio.isPlaying)
        {
            pushPullAudio.loop = true;
            pushPullAudio.Play();
        }
    }

    void StopPushPullAudio()
    {
        if (pushPullAudio != null && pushPullAudio.isPlaying)
        {
            pushPullAudio.Stop();
        }
    }
}
