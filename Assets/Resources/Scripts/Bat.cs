using UnityEngine;
using UnityEngine.UI;

public class Bat : MonoBehaviour, ITakenDamage
{
    #region
    [SerializeField] private float maxHp;
    [SerializeField] private float healthPoint;

    [SerializeField] private float smoothSpeed = 0.005f;
    public Image healthImage;
    public Image delayImage;
    #endregion

    #region
    [Header("Damage Effect")]
    private SpriteRenderer spriteRenderer;
    public float flashLength;
    private float flashCounter;
    #endregion

    private void Start()
    {
        healthPoint = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (flashCounter <= 0)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
        else
        {
            flashCounter -= Time.deltaTime;
        }

        UpdateHealthImage();//TODO Remove from Update
    }

    private void UpdateHealthImage()
    {
        healthImage.fillAmount = healthPoint / maxHp;

        if (delayImage.fillAmount > healthImage.fillAmount)
        {
            delayImage.fillAmount -= smoothSpeed;
        }
        else
        {
            delayImage.fillAmount = healthImage.fillAmount;
        }
    }

    public void TakenDamage(float _damage)
    {
        healthPoint -= _damage;
        Flash();
        if (healthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Flash()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);//"_FlashAmount"名字根据Shader中Properties决定

        flashCounter = flashLength;
    }

}
