using UnityEngine;

public class UIfalse : MonoBehaviour
{
    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true; // 当UI0被SetActive(True)激活时，准备接受输入
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false); // 按空格键关闭自己
            isActive = false; // 防止反复触发
        }
    }
}