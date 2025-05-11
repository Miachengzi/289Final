using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NEW Bullet", fileName = "Bullet")]
public class BulletData : ScriptableObject
{
    public string bulletName;

    public GameObject bulletPrefab;

    public Sprite bulltSprite;
    public Sprite weaponSprite;

    public int bulletNum;
    public float fireRate;
    public float bulletSpeed;
}
