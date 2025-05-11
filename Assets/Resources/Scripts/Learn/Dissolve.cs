using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isDissolving = false;
    float dissolveTime = 1.0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isDissolving = true;
        }

        if(isDissolving)
        {
            dissolveTime -= Time.deltaTime;
            if(dissolveTime <= 0f)
            {
                dissolveTime = 0f;
                isDissolving = true;
            }

            spriteRenderer.material.SetFloat("_Fade", dissolveTime);
            //"_Fade"是Shader Graph中属性的名字啊，通过dissolveTime来改变的
        }
    }

}
