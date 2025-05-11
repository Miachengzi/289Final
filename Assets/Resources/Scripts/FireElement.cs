using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireElement : MonoBehaviour, ITakenDamage
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDist;
    public LayerMask mask;
    public Transform firePoint;
    private Transform target;
    [SerializeField] private Transform targetA, targetB;

    private LineRenderer lineRenderer;
    [SerializeField] private Gradient redColor, greenColor;
    public GameObject laserEffect;

    public float damage;

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

    #region
    [SerializeField] private bool isLeft;
    private int indexNum;
    #endregion

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        target = targetB;
        lineRenderer = GetComponentInChildren<LineRenderer>();

        healthPoint = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(isLeft)
        {
            indexNum = -1;
        }
        else
        {
            indexNum = 1;
        }
    }

    private void Update()
    {
        //Move();
        SinMove();
        Detect();

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

    private void Move()
    {
        if(Vector2.Distance(transform.position, targetA.position) <= 0.1f)
        {
            target = targetB;
        }
        if (Vector2.Distance(transform.position, targetB.position) <= 0.1f)
        {
            target = targetA;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void SinMove()
    {
        transform.position = new Vector2(transform.position.x, (Mathf.Sin(Time.time * moveSpeed) * 4.5f) + 1.5f);
    }

    private void Detect()
    {
        RaycastHit2D hitInfo;

            hitInfo = Physics2D.Raycast(firePoint.position, indexNum * transform.right, maxDist, mask);

            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.tag == "Block")
                {
                    Debug.DrawLine(firePoint.position, hitInfo.point, Color.green);

                    lineRenderer.SetPosition(1, hitInfo.point);
                    lineRenderer.colorGradient = greenColor;
                }
                else if (hitInfo.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(firePoint.position, hitInfo.point, Color.red);

                    lineRenderer.SetPosition(1, hitInfo.point);
                    lineRenderer.colorGradient = redColor;

                    #region 
                    ITakenDamage damagable = hitInfo.transform.GetComponent<ITakenDamage>();
                    if (damagable == null)
                        return;

                    damagable.TakenDamage(damage);
                    #endregion
                }

                Instantiate(laserEffect, hitInfo.point, transform.rotation);
            }

            lineRenderer.SetPosition(0, firePoint.position);
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

}
