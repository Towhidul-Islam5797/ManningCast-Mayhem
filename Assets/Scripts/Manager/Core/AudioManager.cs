#region Summary
/// <summary>
/// AudioManager plays background music and one-shot sound effects for
/// player movement, pickups, and hits. Also exposes volume control
/// methods for the Settings panel's Audio and SFX sliders.
/// </summary>
#endregion

#region Phase 3 Sprint 7 - Audio Manager
//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance { get; private set; }

//    #region Audio Sources
//    [SerializeField] private AudioSource musicSource;
//    [SerializeField] private AudioSource sfxSource;
//    #endregion

//    #region Clips
//    [SerializeField] private AudioClip backgroundMusic;
//    [SerializeField] private AudioClip moveClip;
//    [SerializeField] private AudioClip pickupClip;
//    [SerializeField] private AudioClip bonusScoreClip;
//    [SerializeField] private AudioClip hitClip;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//    }

//    private void Start()
//    {
//        if (backgroundMusic == null) return;

//        musicSource.clip = backgroundMusic;
//        musicSource.loop = true;
//        musicSource.Play();
//    }
//    #endregion

//    #region Sound Effects
//    public void PlayMove()
//    {
//        sfxSource.PlayOneShot(moveClip);
//    }

//    public void PlayPickup()
//    {
//        sfxSource.PlayOneShot(pickupClip);
//    }

//    public void PlayBonusScore()
//    {
//        sfxSource.PlayOneShot(bonusScoreClip);
//    }

//    public void PlayHit()
//    {
//        sfxSource.PlayOneShot(hitClip);
//    }
//    #endregion

//    #region Volume Controls
//    public void SetMusicVolume(float volume)
//    {
//        musicSource.volume = volume;
//    }

//    public void SetSfxVolume(float volume)
//    {
//        sfxSource.volume = volume;
//    }
//    #endregion
//}
#endregion

#region Summary
/// <summary>
/// AudioManager plays background music and one-shot sound effects for
/// player movement, pickups, and hits. Also exposes volume control
/// methods for the Settings panel's Audio and SFX sliders.
/// </summary>
#endregion

#region Phase 3 Sprint 7 - Audio Manager
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region Audio Sources
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    #endregion

    #region Clips
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioClip bonusScoreClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    #endregion

    #region Sound Effects
    public void PlayMove()
    {
        sfxSource.PlayOneShot(moveClip);
    }

    public void PlayPickup()
    {
        sfxSource.PlayOneShot(pickupClip);
    }

    public void PlayBonusScore()
    {
        sfxSource.PlayOneShot(bonusScoreClip);
    }

    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitClip);
    }

    public void PlayDeath()
    {
        sfxSource.PlayOneShot(deathClip);
    }
    #endregion

    #region Volume Controls
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    #endregion
}
#endregion