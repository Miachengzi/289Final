using UnityEngine;

public class inageleftright : MonoBehaviour
{
    public float swaySpeed = 300f;     // ���Ұڶ��ٶ�
    public float swayRange = 10f;     // �ڶ��������루���أ�

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;  // ��¼��ʼλ��
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * swaySpeed * 0.01f) * swayRange;
        transform.localPosition = startPos + new Vector3(offset, 0, 0);
    }
}
