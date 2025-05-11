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
        // ����ģʽ����ֻ֤��һ��AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �糡��������
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

    // ��ѡ����������
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
}