using System.Collections;
using UnityEngine;

public class blink : MonoBehaviour
{
    public float blinkInterval = 0.5f;
    private SpriteRenderer spriteRenderer;
    private bool blinking = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false); // ≥ı ºπÿ±’
    }

    public void StartBlink()
    {
        if (!blinking)
        {
            blinking = true;
            gameObject.SetActive(true);
            StartCoroutine(Blink());
        }
    }

    public void StopBlink()
    {
        blinking = false;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator Blink()
    {
        while (blinking)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        spriteRenderer.enabled = true;
    }
}
