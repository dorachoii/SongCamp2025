using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance { get; private set; }

    public AudioSource audioSource;
    private bool isStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        TryStartSong();
    }

    public void TryStartSong()
    {
        if (!isStarted)
        {
            isStarted = true;
            audioSource.Play();
            Debug.Log("🎵 노래 시작!");
        }
    }
}
