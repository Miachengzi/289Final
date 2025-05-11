using System.Collections;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public static gamemanager Instance;

    public PlayerCollision playerCollision;
    public Flowchart flowchart;

    private bool dialogueStarted = false; // <-new
    private bool isCheckingFlowchart = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        flowchart = FindObjectOfType<Flowchart>();

        if (flowchart != null)
        {
            if (GetIntroPlayed())
            {
                Debug.Log("Intro 已播放，设置 Flowchart 变量为 true");
                flowchart.SetBooleanVariable("IntroPlayed", true);
            }
            else
            {
                Debug.Log("Intro 未播放，准备执行 New Block");
                flowchart.SetBooleanVariable("IntroPlayed", false); // ✅ 强制 false（关键！）
                Block introBlock = flowchart.FindBlock("New Block");

                if (introBlock != null)
                {
                    Debug.Log("成功找到 New Block，开始执行");
                    introBlock.Execute();
                }
            }
        }
    }

    private void Update()
    {
        if (flowchart != null && isCheckingFlowchart)
        {
            if (!dialogueStarted)
            {
                // 对话是否真正开始播放了？（有Executing的Block了）
                if (flowchart.GetExecutingBlocks().Count > 0)
                {
                    dialogueStarted = true;
                }
            }
            else
            {
                // 对话已经开始了 --> 现在监测它是否播放完
                if (flowchart.GetExecutingBlocks().Count == 0)
                {
                    Destroy(flowchart.gameObject);
                    flowchart = null;
                    isCheckingFlowchart = false;
                }
            }
        }
    }

    public void Restart()
    {
        StartCoroutine(RestartWithDelay(2f));
    }

    private IEnumerator RestartWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool GetIntroPlayed()
    {
        return PlayerPrefs.GetInt("IntroPlayed", 0) == 1;
    }

    // 设置为“已经播放过”，只需调用一次
    public void SetIntroPlayed()
    {
        PlayerPrefs.SetInt("IntroPlayed", 1);
        PlayerPrefs.Save(); // 可选但推荐，立即保存到磁盘
    }
}



