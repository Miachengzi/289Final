using UnityEngine;

public class ChangeSwitchArt : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public bool isActivated { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerOrBox(other))
        {
            isActivated = true;
            spriteRenderer.sprite = activeSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayerOrBox(other))
        {
            isActivated = false;
            spriteRenderer.sprite = inactiveSprite;
        }
    }

    // ºÏ≤È « Player ªÚ Box
    private bool IsPlayerOrBox(Collider2D other)
    {
        return other.CompareTag("Player");
    }
}

