using UnityEngine;

public class inageleftright : MonoBehaviour
{
    public float swaySpeed = 300f;     // 左右摆动速度
    public float swayRange = 10f;     // 摆动的最大距离（像素）

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;  // 记录初始位置
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * swaySpeed * 0.01f) * swayRange;
        transform.localPosition = startPos + new Vector3(offset, 0, 0);
    }
}
