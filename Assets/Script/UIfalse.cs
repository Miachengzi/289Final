using UnityEngine;

public class UIfalse : MonoBehaviour
{
    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true; // ��UI0��SetActive(True)����ʱ��׼����������
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false); // ���ո���ر��Լ�
            isActive = false; // ��ֹ��������
        }
    }
}