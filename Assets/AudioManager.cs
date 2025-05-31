using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip chiClip;
    public AudioClip biuClip;

    private AudioSource audioSource;
    private float lastChiTime;
    public float chiCooldown = 0.1f; // ×î¶Ì²¥·Å¼ä¸ô£¨Ãë£©

    private bool initialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            lastChiTime = -999f;
            initialized = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayChi()
    {
        if (!initialized) return;

        if (Time.time - lastChiTime > chiCooldown && chiClip != null)
        {
            audioSource.PlayOneShot(chiClip);
            lastChiTime = Time.time;
        }
    }

    public void PlayBiu()
    {
        if (!initialized) return;

        if (biuClip != null)
        {
            audioSource.PlayOneShot(biuClip);
        }
    }
}
