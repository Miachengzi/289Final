using UnityEngine;
using UnityEngine.SceneManagement;

public class startmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Chapter1");
    }

    public void OpenAchievements()
    {
        SceneManager.LoadScene("AchivementPage");
    }
}

