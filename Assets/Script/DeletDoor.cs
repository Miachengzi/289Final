using Unity.VisualScripting;
using UnityEngine;

public class DeletDoor : MonoBehaviour
{
    public ChangeSwitchArt switch1;
    public ChangeSwitchArt switch2;

    public GameObject opendoor;

    private bool doorOpened = false;

    void Update()
    {
        if (!doorOpened && switch1.isActivated && switch2.isActivated)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        opendoor.GetComponent<AudioSource>().Play();
        gameObject.SetActive(false);
        doorOpened = true;
    }
}
