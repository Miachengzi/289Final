using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image frameImage;

    private void Start()
    {
        frameImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        frameImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        frameImage.gameObject.SetActive(false);
    }

}
