using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BulletData[] bulletDatas;

    public GameObject selectBullet;
    public int index;
    public GameObject[] highlightFrames;

    [SerializeField] private SpriteRenderer weaponSR;
    public GameObject swapEffect;

    public BulletButton[] bulletButtons;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        weaponSR = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<SpriteRenderer>();

        index = 0;
        selectBullet = bulletDatas[index].bulletPrefab;//bullets[index];

        weaponSR.sprite = bulletDatas[index].weaponSprite;//weaponSprites[index];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SelectBullet();
            UpdateSelectedFrame();
            Instantiate(swapEffect, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        }
    }

    private void SelectBullet()
    {
        index++;
        if(index >= bulletDatas.Length)
        {
            index = 0;
        }
        Debug.Log(index);
        selectBullet = bulletDatas[index].bulletPrefab;//bullets[index];

        weaponSR.sprite = bulletDatas[index].weaponSprite;//weaponSprites[index];
    }

    private void UpdateSelectedFrame()
    {
        for(int i = 0; i < highlightFrames.Length; i++)
        {
            if(i == index)
            {
                highlightFrames[i].gameObject.SetActive(true);
                bulletButtons[i].isSelected = true;

                bulletButtons[i].UpdateBulletNum();
            }
            else
            {
                highlightFrames[i].SetActive(false);
                bulletButtons[i].isSelected = false;

                bulletButtons[i].UpdateBulletNum();
            }
        }
    }


}
