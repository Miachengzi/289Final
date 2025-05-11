using UnityEngine;

public class Pixel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float pixelateSpeed = 15f;

    private float pixelAmount;
    private float pixelAmountTarget;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        pixelAmountTarget = 0f;
        pixelAmount = pixelAmountTarget;
    }

    private void Update()
    {
        pixelAmount = Mathf.Lerp(pixelAmount, pixelAmountTarget, Time.deltaTime * pixelateSpeed);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            pixelAmountTarget = 0.95f;//比较极限的像素化效果了，再大就看不清了
        }
           
        if (Input.GetKeyDown(KeyCode.W))
        {
            pixelAmountTarget = 0f;//恢复原来图像的清晰度
        }

        spriteRenderer.material.SetFloat("_PixelateAmount", pixelAmount);

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    pixelAmountTarget = 0.95f;//比较极限的像素化效果了，再大就看不清了
        //    isPixelated = true;
        //}
        //if(isPixelated)
        //    pixelAmount = Mathf.Lerp(pixelAmount, pixelAmountTarget, Time.deltaTime * pixelateSpeed);

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    pixelAmountTarget = 0f;//恢复原来图像的清晰度
        //    isPixelated = false;
        //}
        //if(isPixelated == false)
        //    pixelAmount = Mathf.Lerp(pixelAmountTarget, pixelAmount, Time.deltaTime * pixelateSpeed);

        //spriteRenderer.material.SetFloat("_PixelateAmount", pixelAmount);
    }
}
