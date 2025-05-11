using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    #region
    Vector2 direction;
    [SerializeField] private float offset;
    #endregion

    #region
    [SerializeField] private float maxDist;
    public LayerMask mask;
    public Image boardImage;
    public Text boardText;
    #endregion

    #region
    [Header("Normal Bullet")]
    private GameObject selectedBullet;
    //public GameObject normalBullet;
    public float force = 15f;
    public Transform firePoint;
    public GameObject shotEffect;
    #endregion

    #region 
    [Header("Fast Bullet")]
    [SerializeField] private float shakeAmount;
    [SerializeField] private float sprayRange;
    [SerializeField] private float randomOffset;
    #endregion

    private float timeBtwRate;

    #region
    [Header("Laser")]
    private LineRenderer lineRenderer;
    public Gradient fireColor;
    public LayerMask rayMask;
    public GameObject laserHitWallEffect;
    public GameObject laserShotEffect;
    #endregion

    #region
    [Header("Melo Fake Effect")]
    public GameObject melon;
    public Vector2 groundDispenseVelocity;
    public Vector2 verticalDispenseVelocity;

    public int boomNum;
    public Text boomText;
    #endregion

    private void Start()
    {
        selectedBullet = GameManager.instance.selectBullet;//normalBullet;
        timeBtwRate = GameManager.instance.bulletDatas[GameManager.instance.index].fireRate;

        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.enabled = false;

        boomNum = 20;
        boomText.text = boomNum.ToString();
    }

    private void Update()
    {
        WeaponRotation();
        DetectRaycast();
        InputControl();
    }

    private void InputControl()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum > 0)
        {
            if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "NormalBullet"
                || GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FastBullet"
                || GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FireBullet")
            {
                GameManager.instance.bulletButtons[GameManager.instance.index].UpdateBulletNum();
                Fire();//TODO Fire Rate
            }
        }

        timeBtwRate -= Time.deltaTime;
        if (Input.GetMouseButton(1) && timeBtwRate <= 0 && GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum > 0)
        {
            if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "NormalBullet"
                || GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FastBullet"
                || GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FireBullet")
            {
                GameManager.instance.bulletButtons[GameManager.instance.index].UpdateBulletNum();
                randomOffset = Random.Range(-10, 10);
                Fire();
                timeBtwRate = GameManager.instance.bulletDatas[GameManager.instance.index].fireRate;
            }
        }

        //MARKER 激光武器 LASER
        if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "LaserBullet")
        {
            if (Input.GetMouseButton(0) && GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum > 0)
            {
                GameManager.instance.bulletButtons[GameManager.instance.index].UpdateBulletNum();
                LaserRay();
            }

            if (Input.GetMouseButtonUp(0) || GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum == 0)
            {
                GameManager.instance.bulletButtons[GameManager.instance.index].UpdateBulletNum();
                StopRay();
            }
        }

        //TODO DELETE LATER
        //if (Input.GetKeyDown(KeyCode.Space))
        if(Input.GetMouseButtonDown(2))
        {
            boomNum--;
            boomText.text = boomNum.ToString();

            GameObject melonGo = Instantiate(melon, firePoint.position, Quaternion.identity);
            melonGo.GetComponent<FakeHeight>().Initialize(firePoint.right * Random.Range(groundDispenseVelocity.x, groundDispenseVelocity.y), 
                                                          Random.Range(verticalDispenseVelocity.x, verticalDispenseVelocity.y));
        }
    }

    private void WeaponRotation()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;//target Pos - current Pos
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ + offset);
    }

    //MARKER 武器枪口路径上，检测敌人显示UI信息
    private void DetectRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, maxDist, mask);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "Block")
            {
                boardImage.gameObject.SetActive(false);
                boardText.text = "";

                Debug.DrawLine(firePoint.position, hit.point, Color.green);
            } 
            else if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                boardImage.gameObject.SetActive(true);
                boardText.text = hit.collider.gameObject.name;

                Debug.DrawLine(firePoint.position, hit.point, Color.red);
            }
            else
            {
                boardImage.gameObject.SetActive(false);
                boardText.text = "";

                Debug.DrawLine(firePoint.position, direction * maxDist, Color.green);
            }
        }
    }

    private GameObject SpawnNewBullet()
    {
        GameObject newBullet = Instantiate(GameManager.instance.selectBullet, firePoint.position, firePoint.rotation);
        GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum--;
        Debug.Log("Bullet NAME: " + GameManager.instance.bulletDatas[GameManager.instance.index].bulletName);
        return newBullet;
    }

    private void Fire()
    {
        GameManager.instance.bulletButtons[GameManager.instance.index].UpdateBulletNum();

        if(GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "NormalBullet")
        {
            Instantiate(shotEffect, firePoint.position, transform.rotation);//发射时的效果

            GameObject newBullet = SpawnNewBullet();//创建子弹游戏对象，每次射击子弹数量-1
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();//获取RB组件

            rb.AddForce(firePoint.right * force, ForceMode2D.Impulse);//MARKER 给子弹一个瞬间的力道 ForceMode2D.Force则是添加一个可持续的力道
        }

        if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FastBullet")
        {
            FindObjectOfType<CameraController>().ScreenShake(shakeAmount);
            Instantiate(shotEffect, firePoint.position, transform.rotation);

            GameObject newBullet = SpawnNewBullet();
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

            rb.AddForce(firePoint.right * force + new Vector3(randomOffset / 3, randomOffset / 3, 0), ForceMode2D.Impulse);//MARKER
        }

        //TODO UNCOMPLETED
        if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "FireBullet")
        { 
            Instantiate(shotEffect, firePoint.position, transform.rotation);

            GameObject newBullet = SpawnNewBullet();
            //newBullet.GetComponent<HomingBullet>().UpdateBullet();
            //Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            //rb.AddForce(firePoint.right * force, ForceMode2D.Impulse);
        }

        //MARKER SPECIAL SPECIAL SPECIAL
        if (GameManager.instance.bulletDatas[GameManager.instance.index].bulletName == "LaserBullet")
        {
            Instantiate(shotEffect, firePoint.position, transform.rotation);

            GameObject newBullet = SpawnNewBullet();
        }
    }

    private void LaserRay()
    {
        lineRenderer.enabled = true;//启用LR组件
        GameManager.instance.bulletDatas[GameManager.instance.index].bulletNum--;//减少子弹

        FindObjectOfType<CameraController>().ScreenShake(shakeAmount / 2);//OPTIONAL 相机震动

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, direction, 20, rayMask);
        if(hitInfo.collider != null)
        {
            lineRenderer.SetPosition(1, hitInfo.point);
            lineRenderer.colorGradient = fireColor;

            Instantiate(laserShotEffect, firePoint.position, transform.rotation);
            Instantiate(laserHitWallEffect, hitInfo.point, transform.rotation);
        }

        lineRenderer.SetPosition(0, firePoint.position);
    }

    private void StopRay()
    {
        lineRenderer.enabled = false;
    }


}
