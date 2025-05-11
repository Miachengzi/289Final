using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
    public GameObject FruitIcon;
    public GameObject GemIcon;
    public GameObject FullHealthIcon;

    void Start()
    {
        if (PlayerPrefs.GetInt("FruitAchievementUnlocked", 0) == 1)
        {
            FruitIcon.SetActive(true);
        }

        if (PlayerPrefs.GetInt("GemAchievementUnlocked", 0) == 1)
        {
            GemIcon.SetActive(true);
        }

        if (PlayerPrefs.GetInt("FullHealthAchievementUnlocked", 0) == 1)
        {
            FullHealthIcon.SetActive(true);
        }
    }
}

