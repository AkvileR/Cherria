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
    public AudioClip takeDamage;
    public AudioClip broccoliDie;

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

    public static void PlayTakeDamage()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayTakeDamage();
    }

    private void InstancePlayTakeDamage()
    {
        PlayFxClip(takeDamage);
    }

    public static void PlayBroccoliDie()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayBroccoliDie();
    }

    private void InstancePlayBroccoliDie()
    {
        PlayFxClip(broccoliDie);
    }
}