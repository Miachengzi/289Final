using UnityEngine;

public class Blur : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool isBlur;
    private float blurAmount;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isBlur = !isBlur;
        }

        float blurSpeed = 10f;//这个数值5，在这个效果里已经是极限的效了，不然会超出图片原始范【除非你的图片空余空间很多】
        if (isBlur)
            blurAmount += blurSpeed * Time.deltaTime;
        else
            blurAmount -= blurSpeed * Time.deltaTime;

        blurAmount = Mathf.Clamp(blurAmount, 0f, 10f);//保证在（0，1）这个区间内
        spriteRenderer.material.SetFloat("_BlurAmount", blurAmount);
    }

}
