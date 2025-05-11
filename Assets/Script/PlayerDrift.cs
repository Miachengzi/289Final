using UnityEngine;

public class PlayerDrift : MonoBehaviour
{
    public float floatAmplitude = 0.1f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
