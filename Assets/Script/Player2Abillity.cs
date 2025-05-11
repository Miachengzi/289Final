using UnityEngine;

public class Player2Abillity : MonoBehaviour
{
    public GameObject wave;

    void Update()
    {
        if (wave.activeSelf)
        {
            wave.transform.position = transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            wave.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Keypad0))
        {
            wave.SetActive(false);
        }
    }
}
