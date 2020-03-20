using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float pitchRandomness = 0.2f;
    private float originalFXPitch;

    [Header("Clips")]
    public AudioClip jump;
    public AudioClip die;
    public AudioClip win;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        originalFXPitch = sfxSource.pitch;
        transform.SetParent(Camera.main.transform);
    }

    private void PlayFxClip(AudioClip clip)
    {
        sfxSource.pitch = originalFXPitch + Random.Range(-pitchRandomness, pitchRandomness);
        sfxSource.PlayOneShot(clip);
    }

    // All the clips
    public static void PlayJump()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayJump();
    }

    private void InstancePlayJump()
    {
        PlayFxClip(jump);
    }

    public static void PlayDie()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayDie();
    }

    private void InstancePlayDie()
    {
        PlayFxClip(die);
    }

    public static void PlayWin()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayWin();
    }

    private void InstancePlayWin()
    {
        PlayFxClip(win);
    }
}