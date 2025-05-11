using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class audiomanager : MonoBehaviour
{
    public static audiomanager Instance;

    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioClip bgmClip;

    void Awake()
    {
        // 单例模式，保证只有一个AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicSource != null && bgmClip != null)
        {
            musicSource.clip = bgmClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // 可选：控制音量
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
}