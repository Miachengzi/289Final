using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float tileSize = 1f;
    public float moveAmount = 5f;
    public float edgeThreshold = 2f;
    public float moveSpeed = 5f;

    private Vector3 cameraAnchor;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        cameraAnchor = transform.position;
        targetPosition = cameraAnchor;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
            return;
        }

        Vector3 center = cameraAnchor;
        Vector3 offset1 = player1.position - center;
        Vector3 offset2 = player2.position - center;

        float threshold = edgeThreshold * tileSize;

        if (offset1.x > threshold && offset2.x > threshold)
        {
            MoveCamera(Vector3.right);
        }
        else if (offset1.x < -threshold && offset2.x < -threshold)
        {
            MoveCamera(Vector3.left);
        }
        else if (offset1.y > threshold && offset2.y > threshold)
        {
            MoveCamera(Vector3.up);
        }
        else if (offset1.y < -threshold && offset2.y < -threshold)
        {
            MoveCamera(Vector3.down);
        }
    }

    void MoveCamera(Vector3 direction)
    {
        cameraAnchor += direction * moveAmount * tileSize;
        targetPosition = cameraAnchor;
        isMoving = true;
    }
}
