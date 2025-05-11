using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextLevel : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public GameObject victoryUI;

    public AudioSource victorySound;
    public AudioSource failureSound;

    private bool player1Inside = false;
    private bool player2Inside = false;
    private bool triggered = false;
    private bool canProceedToNextLevel = false;

    public GameObject FullHealthAchievementUI;

    void Update()
    {
        if (canProceedToNextLevel && Input.GetKeyDown(KeyCode.Space))
        {
            GoToNextScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player1)
            player1Inside = true;
        else if (other.gameObject == player2)
            player2Inside = true;

        CheckCondition();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player1)
            player1Inside = false;
        else if (other.gameObject == player2)
            player2Inside = false;
    }

    void CheckCondition()
    {
        if (player1Inside && player2Inside)
        {
            triggered = true;

            if (PlayerCollision.GetGemCount() == 3)
            {
                if (victorySound != null) victorySound.Play();
                if (victoryUI != null) victoryUI.SetActive(true);

                canProceedToNextLevel = true;

                PlayerCollision player1Script = player1.GetComponent<PlayerCollision>();
                PlayerCollision player2Script = player2.GetComponent<PlayerCollision>();

                if (player1Script != null && player2Script != null && player1Script.IsFullHealth() && player2Script.IsFullHealth())
                {
                    if (FullHealthAchievementUI != null)
                    {
                        FullHealthAchievementUI.SetActive(true);
                    }

                    PlayerPrefs.SetInt("FullHealthAchievementUnlocked", 1);
                    PlayerPrefs.Save();
                }
            }

            else
            {
                if (failureSound != null) failureSound.Play();
            }
        }
    }
    void GoToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes!"); // 如果超出Build Settings里的关卡数量
        }
    }
}
