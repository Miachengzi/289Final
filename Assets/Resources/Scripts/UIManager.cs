using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    public Animator inventoryAnimator;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
        }

        inventoryAnimator.SetBool("isActive", isOpen);
    }
}
