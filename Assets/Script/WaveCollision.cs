using UnityEngine;

public class WaveCollision : MonoBehaviour
{
    public Transform playerToFollow;  // ÍÏ×§ Player2 µÄ Transform
    public GameObject hitSoundObject;

    private void Update()
    {
        if (playerToFollow != null)
        {
            transform.position = playerToFollow.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemybaby"))
        {
            if (hitSoundObject != null)
            {
                AudioSource audio = hitSoundObject.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }
            }

            Destroy(other.gameObject);
        }
    }
    
}
