using UnityEngine;

public class Tint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color materialTintColor;
    [SerializeField] private float tintFadeSpeed;
    [SerializeField] private Color tintColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialTintColor = spriteRenderer.material.color;
    }

    private void Update()
    {
        if(spriteRenderer.material.color.a > 0)
        {
            materialTintColor.a = Mathf.Clamp01(materialTintColor.a - tintFadeSpeed * Time.deltaTime);
            spriteRenderer.material.SetColor("_Tint", materialTintColor);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            materialTintColor = tintColor;
            spriteRenderer.material.SetColor("_Tint", materialTintColor);
        }
    }
}
