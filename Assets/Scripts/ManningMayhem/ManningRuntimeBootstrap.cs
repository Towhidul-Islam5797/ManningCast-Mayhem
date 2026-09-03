using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Installs the revised client flow without destructively rewriting the existing scene art.
/// The studio environment remains in place; legacy pools and UI are disabled at runtime.
/// </summary>
public sealed class ManningRuntimeBootstrap : MonoBehaviour
{
    public static bool IsInstalled { get; private set; }

    private string configuredSceneName;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        IsInstalled = true;
        if (FindAnyObjectByType<ManningRuntimeBootstrap>() != null) return;
        if (PlayerPrefs.HasKey("Manning.SelectedCharacter"))
        {
            CharacterSelection.SelectedCharacter = (CharacterSelection.Character)Mathf.Clamp(PlayerPrefs.GetInt("Manning.SelectedCharacter"), 0, 1);
        }

        GameObject bootstrap = new GameObject("ManningRuntimeBootstrap");
        DontDestroyOnLoad(bootstrap);
        bootstrap.AddComponent<ManningRuntimeBootstrap>();
        ManningAudio.Ensure();
    }

    private void Awake()
    {
        IsInstalled = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        ConfigureScene(SceneManager.GetActiveScene());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        configuredSceneName = null;
        ConfigureScene(scene);
    }

    private void ConfigureScene(Scene scene)
    {
        if (!scene.IsValid() || scene.name == configuredSceneName) return;
        configuredSceneName = scene.name;

        switch (scene.name)
        {
            case "MainMenu":
                DisableSceneCanvases(scene);
                if (FindAnyObjectByType<ManningFrontEndFlow>(FindObjectsInactive.Include) == null)
                    new GameObject("ManningFrontEndFlow (Runtime Fallback)").AddComponent<ManningFrontEndFlow>();
                break;

            case "CharacterSelectScene":
                DisableSceneCanvases(scene);
                if (FindAnyObjectByType<ManningCharacterSelectFlow>(FindObjectsInactive.Include) == null)
                    new GameObject("ManningCharacterSelectFlow (Runtime Fallback)").AddComponent<ManningCharacterSelectFlow>();
                break;

            case "GameScene":
                ConfigureGameScene(scene);
                break;
        }
    }

    private static void ConfigureGameScene(Scene scene)
    {
        DisableSceneCanvases(scene);
        DisableLegacyGameplay(scene);

        GameManager game = GameManager.Instance;
        if (game == null)
        {
            game = new GameObject("GameManager_Runtime").AddComponent<GameManager>();
        }
        game.ResetRound();

        if (PauseManager.Instance == null)
        {
            new GameObject("PauseManager_Runtime").AddComponent<PauseManager>();
        }

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            ManningCharacterSpriteAnimator characterAnimator = player.GetComponent<ManningCharacterSpriteAnimator>();
            if (characterAnimator == null) characterAnimator = player.gameObject.AddComponent<ManningCharacterSpriteAnimator>();
            characterAnimator.Initialize(CharacterSelection.SelectedCharacter);
            player.ConfigureRuntimeArt(characterAnimator);
            player.BindGameManager();
        }

        ManningLaneDirector lanes = FindAnyObjectByType<ManningLaneDirector>(FindObjectsInactive.Include);
        if (lanes == null)
            lanes = new GameObject("ManningSevenLaneDirector (Runtime Fallback)").AddComponent<ManningLaneDirector>();

        ManningCouchSpectator couchSpectator = FindAnyObjectByType<ManningCouchSpectator>(FindObjectsInactive.Include);
        if (couchSpectator == null)
        {
            GameObject spectator = new GameObject("Opposite Brother - Couch Spectator (Runtime Fallback)");
            spectator.transform.position = new Vector3(0.48f, 3.2f, 0f);
            couchSpectator = spectator.AddComponent<ManningCouchSpectator>();
        }
        CharacterSelection.Character opposite = CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton
            ? CharacterSelection.Character.Eli
            : CharacterSelection.Character.Peyton;
        couchSpectator.Initialize(opposite);

        ManningGameUI gameUI = FindAnyObjectByType<ManningGameUI>(FindObjectsInactive.Include);
        if (gameUI == null) gameUI = new GameObject("ManningGameUIController (Runtime Fallback)").AddComponent<ManningGameUI>();
        gameUI.BindGameManager();
        ManningAudio.Ensure().Bind(game);
    }

    private static void DisableSceneCanvases(Scene scene)
    {
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.scene == scene && !canvas.name.StartsWith("Manning")) canvas.enabled = false;
        }
    }

    private static void DisableLegacyGameplay(Scene scene)
    {
        DisableNamedRoot("Pools");
        DisableNamedRoot("ObstacleLanes");
        DisableNamedRoot("SafeLane");
        DisableNamedRoot("SafeLane_1");
        DisableNamedRoot("SafeLane_2");
        DisableNamedRoot("HazardZone_1");
        DisableNamedRoot("HazardZone_2");

        foreach (ObstacleSpawner component in FindObjectsByType<ObstacleSpawner>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.gameObject.SetActive(false);
        }
        foreach (ObjectPool component in FindObjectsByType<ObjectPool>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.gameObject.SetActive(false);
        }
        foreach (Obstacle component in FindObjectsByType<Obstacle>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.gameObject.SetActive(false);
        }
        foreach (SafeObject component in FindObjectsByType<SafeObject>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.gameObject.SetActive(false);
        }
        foreach (HUDManager component in FindObjectsByType<HUDManager>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.enabled = false;
        }
        foreach (GameOverUI component in FindObjectsByType<GameOverUI>(FindObjectsInactive.Include))
        {
            if (component.gameObject.scene == scene) component.enabled = false;
        }
    }

    private static void DisableNamedRoot(string name)
    {
        GameObject target = GameObject.Find(name);
        if (target != null) target.SetActive(false);
    }
}
