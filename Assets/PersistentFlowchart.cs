using UnityEngine;

public class PersistentFlowchart : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
