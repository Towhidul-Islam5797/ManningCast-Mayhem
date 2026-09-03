using UnityEngine;

/// <summary>Persistent music and event-driven SFX using the audio supplied in Assets.</summary>
public sealed class ManningAudio : MonoBehaviour
{
    private const string MusicVolumeKey = "Manning.Audio.MusicVolume";
    private const string SfxVolumeKey = "Manning.Audio.SfxVolume";
    private const float MusicCeiling = 0.32f;
    private const float SfxCeiling = 0.72f;

    public static ManningAudio Instance { get; private set; }

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioClip moveClip;
    private AudioClip hitClip;
    private AudioClip bonusClip;
    private AudioClip pickupClip;
    private AudioClip scoreClip;
    private GameManager boundGame;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    /// <summary>Normalised 0-1 music level, persisted between sessions.</summary>
    public float MusicVolume => musicVolume;

    /// <summary>Normalised 0-1 effects level, persisted between sessions.</summary>
    public float SfxVolume => sfxVolume;

    public static ManningAudio Ensure()
    {
        if (Instance != null) return Instance;
        GameObject audioObject = new GameObject("ManningAudio");
        Instance = audioObject.AddComponent<ManningAudio>();
        DontDestroyOnLoad(audioObject);
        return Instance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        musicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MusicVolumeKey, 1f));
        sfxVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(SfxVolumeKey, 1f));

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        ApplyVolumes();

        musicSource.clip = ManningAssetLibrary.LoadAudio("Music");
        moveClip = ManningAssetLibrary.LoadAudio("Move");
        hitClip = ManningAssetLibrary.LoadAudio("Hit");
        bonusClip = ManningAssetLibrary.LoadAudio("Bonus");
        pickupClip = ManningAssetLibrary.LoadAudio("Pickup");
        scoreClip = ManningAssetLibrary.LoadAudio("Score");
        if (musicSource.clip != null) musicSource.Play();
    }

    private void OnDestroy()
    {
        Unbind();
        if (Instance == this) Instance = null;
    }

    public void Bind(GameManager game)
    {
        if (boundGame == game) return;
        Unbind();
        boundGame = game;
        if (boundGame == null) return;
        boundGame.StateChanged += OnStateChanged;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = MusicCeiling * musicVolume;
        if (sfxSource != null) sfxSource.volume = SfxCeiling * sfxVolume;
    }

    private void Unbind()
    {
        if (boundGame != null) boundGame.StateChanged -= OnStateChanged;
        boundGame = null;
    }

    private void OnStateChanged(GameManager.GameState state)
    {
        PlayOneShot(state == GameManager.GameState.Won ? scoreClip : hitClip, 1f);
    }

    public void PlayMove() => PlayOneShot(moveClip, 0.32f);
    public void PlayHit() => PlayOneShot(hitClip, 0.95f);
    public void PlayBonus() => PlayOneShot(bonusClip, 0.9f);
    public void PlayPickup() => PlayOneShot(pickupClip, 0.7f);
    public void PlayUi() => PlayOneShot(pickupClip, 0.35f);

    private void PlayOneShot(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null) sfxSource.PlayOneShot(clip, volume * sfxVolume);
    }
}
