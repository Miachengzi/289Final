using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;


public class PlayerCollision : MonoBehaviour
{
    public GameObject Injure;
    public GameObject Lose;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    private int health = 3;//no share

    public GameObject Strawberry;
    public GameObject RedApple;
    public GameObject GreenApple;
    public GameObject Lemon;
    public GameObject Blueberry;
    public GameObject Cherry;
    
    public GameObject FruitAchievement;

    public GameObject GetSound;
    public GameObject GetAchievementSound;

    private static int fruit = 0;//share

    public GameObject GetGem;
    public GameObject GemCount;
    public TextMeshProUGUI gemcount;

    public GameObject GemAchievement;

    private static int gemCount = 0;
    public static int GetGemCount()
    {
        return gemCount;
    }

    public TextMeshProUGUI potioncount;

    private int potionCount = 0;

    public GameObject RecoverSound;



    private GameObject currentFruitUI = null;
    private bool isFruitUIActive = false;

    public MonoBehaviour player1MoveScript;
    public Player2Move player2MoveScript;
    public Player2Abillity player2AbilityScript;
    private MonoBehaviour[] allEnemyScripts;

    private static HashSet<string> collectedItems = new HashSet<string>();

    public PlayerCollision otherPlayerCollision;

    public GameObject FinalRequestGreen;
    public GameObject FinalRequestRed;


    public bool IsFullHealth()
    {
        return health == 3;
    }

    void Start()
    {
        foreach (string itemName in collectedItems)
        {
            GameObject obj = GameObject.Find(itemName);
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        UpdateGemUI(); // 不要重置 gemCount = 0;

        // 自动收集所有类型的敌人脚本
        var enemy1 = FindObjectsOfType<Enemy1Move>();
        var enemy2 = FindObjectsOfType<Enemy2Move>();
        var enemy3 = FindObjectsOfType<Enemy3Move>();

        // 合并为一个统一的控制数组
        List<MonoBehaviour> enemies = new List<MonoBehaviour>();
        enemies.AddRange(enemy1);
        enemies.AddRange(enemy2);
        enemies.AddRange(enemy3);

        allEnemyScripts = enemies.ToArray();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && potionCount > 0 && (health < 3 || (otherPlayerCollision != null && otherPlayerCollision.health < 3)))
        {
            RestoreHealth();
            if (otherPlayerCollision != null)
            {
                otherPlayerCollision.RestoreHealth();
            }

            potionCount--;
            UpdatePotionUI();
        }

        if (isFruitUIActive && Input.GetKeyDown(KeyCode.Space))
        {
            currentFruitUI.SetActive(false);
            currentFruitUI = null;
            isFruitUIActive = false;
            EnableAllControl(true); // 恢复移动
            SetEnemyScriptsActive(true);
        }
    }


     
    void EnableAllControl(bool enable)
    {
        if (player1MoveScript != null) player1MoveScript.enabled = enable;
        if (player2MoveScript != null) player2MoveScript.enabled = enable;
        if (player2AbilityScript != null) player2AbilityScript.enabled = enable;
    }

    void SetEnemyScriptsActive(bool active)
    {
        if (allEnemyScripts == null) return;

        foreach (var script in allEnemyScripts)
        {
            script.enabled = active;
        }
    }

    public void RestoreHealth()
    {
        if (health < 3)
        {
            if (heart1 != null) heart1.SetActive(true);
            if (heart2 != null) heart2.SetActive(true);
            if (heart3 != null) heart3.SetActive(true);
            health = 3;

            RecoverSound.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator ShowAchievementAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        GetAchievementSound.GetComponent<AudioSource>().Play();

        FruitAchievement.SetActive(true);
        yield return new WaitForSeconds(4f);
        FruitAchievement.SetActive(false);

        PlayerPrefs.SetInt("FruitAchievementUnlocked", 1);
        PlayerPrefs.Save();

    }

    IEnumerator ShowGemAchievementAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        GetAchievementSound.GetComponent<AudioSource>().Play();

        GemAchievement.SetActive(true);
        yield return new WaitForSeconds(4f);
        GemAchievement.SetActive(false);

        PlayerPrefs.SetInt("GemAchievementUnlocked", 1);
        PlayerPrefs.Save();

    }

    void ShowUI(GameObject fruitUI)
    {
        if (fruitUI == null) return;

        fruitUI.SetActive(true);
        currentFruitUI = fruitUI;
        isFruitUIActive = true;
        EnableAllControl(false); // 禁用移动
        SetEnemyScriptsActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("enemybaby"))
        {
            TakeDamage(); 
        }

        if (other.gameObject.CompareTag("potion"))
        {
            potionCount++;
            Destroy(other.gameObject);
            GetSound.GetComponent<AudioSource>().Play();

            UpdatePotionUI();
        }
        
        if (other.gameObject.CompareTag("fruit"))
        {
            if (collectedItems.Contains(other.name)) return; // 跳过重复收集

            collectedItems.Add(other.name); 

            switch (other.name)
            {
                case "Strawberry":
                    ShowUI(Strawberry);
                    break;
                case "RedApple":
                    ShowUI(RedApple);
                    break;
                case "GreenApple":
                    ShowUI(GreenApple);
                    break;
                case "Lemon":
                    ShowUI(Lemon);
                    break;
                case "Blueberry":
                    ShowUI(Blueberry);
                    break;
                case "Cherry":
                    ShowUI(Cherry);
                    break;
            }

            fruit++;
            Destroy(other.gameObject);
            GetSound.GetComponent<AudioSource>().Play();

            if (fruit == 6)
            {
                StartCoroutine(ShowAchievementAfterDelay());
            }
        }

        if (other.gameObject.CompareTag("gem"))
        {
            if (collectedItems.Contains(other.name)) return;

            collectedItems.Add(other.name); 
            
            gemCount++;
            Destroy(other.gameObject);
            GetSound.GetComponent<AudioSource>().Play();

            if (gemCount == 1)
            {
                ShowUI(GetGem);
                GemCount.SetActive(true);
            }

            if (gemCount == 3)
            {
                StartCoroutine(ShowGemAchievementAfterDelay());
            }

            UpdateGemUI();
        }

        if (other.gameObject.CompareTag("narration"))
        {
            if (collectedItems.Contains(other.name)) return;

            collectedItems.Add(other.name); 
            
            switch (other.name)
            {
                
                case "Collider2":
                    FindObjectOfType<TutorialsController>().TriggerNarration(1);
                    break;
                case "Collider3":
                    FindObjectOfType<TutorialsController>().TriggerNarration(2);
                    break;
                case "Collider4":
                    FindObjectOfType<TutorialsController>().TriggerNarration(3);
                    break;
                case "Collider5":
                    FindObjectOfType<TutorialsController>().TriggerNarration(4);
                    break;
                case "Collider6":
                    FindObjectOfType<TutorialsController>().TriggerNarration(5);
                    break;
                case "Collider7":
                    FindObjectOfType<TutorialsController>().TriggerNarration(6);
                    break;
                case "Collider8":
                    FindObjectOfType<TutorialsController>().TriggerNarration(7);
                    break;
                case "Collider9":
                    FindObjectOfType<TutorialsController>().TriggerNarration(8);
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    void UpdateGemUI()
    {
        if (gemcount != null)
        {
            gemcount.text = "x" + gemCount.ToString();
        }

        if (gemCount == 3 && FinalRequestGreen != null && !FinalRequestGreen.activeSelf)
        {
            FinalRequestGreen.SetActive(true);
            FinalRequestRed.SetActive(false);
        }
    }

    void UpdatePotionUI()
    {
        if (potioncount != null)
        {
            potioncount.text = "x" + potionCount.ToString();
        }
    }

    void TakeDamage()
    {
        health--;

        if (health == 2 && heart3 != null)
        {
            Injure.GetComponent<AudioSource>().Play();
            heart3.SetActive(false);
        }
        else if (health == 1 && heart2 != null)
        {
            Injure.GetComponent<AudioSource>().Play();
            heart2.SetActive(false);
        }
        else if (health == 0 && heart1 != null)
        {
            Lose.GetComponent<AudioSource>().Play();
            heart1.SetActive(false);
            gameObject.SetActive(false);
            gamemanager.Instance.Restart();
        }
    }
}
