using System.Collections.Generic;
using UnityEngine;

public class TutorialsController : MonoBehaviour
{
    public MonoBehaviour player1MoveScript;
    public Player2Move player2MoveScript;
    public Player2Abillity player2AbilityScript;

    private MonoBehaviour[] allEnemyScripts;

    public GameObject uiBackground;
    public GameObject[] narrationUIs;
    
    private GameObject currentUI;

    private bool isNarrating = false;

    public GameObject ui2Point5;
    private bool hasSwitchedToSecond = false;

    public GameObject ui5Point5;
    private bool hasSwitchedToFiveFive = false;

    public GameObject ui8Point5;
    private bool hasSwitchedToEightFive = false;

    public blink blinkEffect;

    public blink blinkEffect8;   // 对应 UI8 的 slide
    public blink blinkEffect8_5; // 对应 UI8.5 的 slide

    public blink blinkEffect9;

    void Start()
    {
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
        if (isNarrating && Input.GetKeyDown(KeyCode.Space))
        {
            if (currentUI == narrationUIs[1] && !hasSwitchedToSecond && ui2Point5 != null)
            {
                currentUI.SetActive(false);
                ui2Point5.SetActive(true);
                currentUI = ui2Point5;
                hasSwitchedToSecond = true;
                return;
            }

            if (currentUI == narrationUIs[4] && !hasSwitchedToFiveFive && ui5Point5 != null)
            {
                currentUI.SetActive(false);
                ui5Point5.SetActive(true);
                currentUI = ui5Point5;
                hasSwitchedToFiveFive = true;
                return;
            }

            if (currentUI == narrationUIs[7] && !hasSwitchedToEightFive && ui8Point5 != null)
            {
                currentUI.SetActive(false);
                ui8Point5.SetActive(true);
                currentUI = ui8Point5;
                hasSwitchedToEightFive = true;

                // 停止 UI8 的 slide 闪烁
                if (blinkEffect8 != null) blinkEffect8.StopBlink();

                // 开启 UI8.5 的 slide 闪烁
                if (blinkEffect8_5 != null) blinkEffect8_5.StartBlink();

                if (blinkEffect9 != null) blinkEffect9.StartBlink();

                return;
            }

            EndNarration();
        }
    }

    public void TriggerNarration(int index)
    {
        if (index < 0 || index >= narrationUIs.Length) return;

        currentUI = narrationUIs[index];
        currentUI.SetActive(true);
        uiBackground.SetActive(true);
        isNarrating = true;

        hasSwitchedToSecond = false; // 每次触发都重置状态
        hasSwitchedToFiveFive = false;

        EnableAllControl(false);
        SetEnemyScriptsActive(false);
        hasSwitchedToEightFive = false;

        if (index == 4 && blinkEffect != null)
            blinkEffect.StartBlink();

        // UI8 闪烁
        if (index == 7 && blinkEffect8 != null)
            blinkEffect8.StartBlink();

        if (index == 8 && blinkEffect9 != null)
            blinkEffect9.StartBlink();
    }

    private void EndNarration()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        uiBackground.SetActive(false);
        isNarrating = false;

        EnableAllControl(true);
        SetEnemyScriptsActive(true);

        if (blinkEffect != null) blinkEffect.StopBlink();
        if (blinkEffect8 != null) blinkEffect8.StopBlink();
        if (blinkEffect8_5 != null) blinkEffect8_5.StopBlink();
        if (blinkEffect9 != null) blinkEffect9.StopBlink();
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
}
