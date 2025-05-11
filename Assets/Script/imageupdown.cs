using UnityEngine;

public class imageupdown : MonoBehaviour
{
    public float floatSpeed = 30f;
    public float floatRange = 10f;

    private Vector3 startPos;
    private float direction = 1f;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed * 0.01f) * floatRange;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }

}
