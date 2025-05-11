using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, ITakenDamage
{
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private float smoothSpeed = 0.005f;

    public Image healthImage;
    public Image delayImage;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        UpdateHealthImage();//TODO Remove from Update
    }

    public void TakenDamage(float _damage)
    {
        health -= _damage;
    }

    private void UpdateHealthImage()
    {
        healthImage.fillAmount = health / maxHealth;

        if(delayImage.fillAmount > healthImage.fillAmount)
        {
            delayImage.fillAmount -= smoothSpeed;
        }
        else
        {
            delayImage.fillAmount = healthImage.fillAmount;
        }
    }

}
