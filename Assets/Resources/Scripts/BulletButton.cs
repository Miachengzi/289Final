using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletButton : MonoBehaviour
{
    public BulletData bulletData;
    public bool isSelected;

    public Image bulletImage;
    public Text bulletNumText;

    private void Start()
    {
        if(!isSelected)
        {
            bulletImage.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 1f);
        }
        else
        {
            bulletImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        UpdateBulletNum();
    }

    //MARKER THIS FUNCTION will be called GM's UpdateSelectedFrame function
    public void UpdateBulletNum()
    {
        bulletNumText.text = bulletData.bulletNum.ToString();

        if(isSelected)
        {
            if(bulletData.bulletNum != 0)
            {
                bulletImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                bulletImage.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 1f); //MARKER OUT OF AMMO
            }
        }
        else
        {
            bulletImage.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 1f);
        }
    }
}
