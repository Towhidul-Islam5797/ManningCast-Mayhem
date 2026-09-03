using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Updates the hierarchy-authored HUD, pause screen, and end screen. Runtime construction is kept
/// only as a fallback for older scene copies; production UI remains directly editable in Hierarchy.
/// </summary>
public sealed class ManningGameUI : MonoBehaviour
{
    [Header("Client Font and Canvas")]
    [SerializeField] private Font brandFont;
    [SerializeField] private Canvas canvas;

    [Header("HUD")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text footballText;
    [SerializeField] private Text toastText;

    [Header("Overlays")]
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject endOverlay;
    [SerializeField] private RawImage endArtwork;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text leaderboardText;

    private GameManager subscribedGame;
    private Coroutine toastRoutine;
    private bool endShown;

    public bool IsHierarchyConfigured => canvas != null && scoreText != null && timeText != null && livesText != null &&
        footballText != null && toastText != null && pauseOverlay != null && endOverlay != null && endArtwork != null;

    public void ConfigureHierarchy(Font font, Canvas authoredCanvas, Text score, Text time, Text lives, Text football,
        Text toast, GameObject pause, GameObject end, RawImage artwork, Text finalScore, Text leaderboard)
    {
        brandFont = font;
        canvas = authoredCanvas;
        scoreText = score;
        timeText = time;
        livesText = lives;
        footballText = football;
        toastText = toast;
        pauseOverlay = pause;
        endOverlay = end;
        endArtwork = artwork;
        finalScoreText = finalScore;
        leaderboardText = leaderboard;
    }

    private void Awake()
    {
        ManningUIFactory.SetFontFamily(brandFont);
        if (!IsHierarchyConfigured) BuildRuntimeHierarchy();
        if (pauseOverlay != null) pauseOverlay.SetActive(false);
        if (endOverlay != null) endOverlay.SetActive(false);
        PauseManager.PauseChanged += OnPauseChanged;
    }

    private void Start()
    {
        BindGameManager();
    }

    private void OnDestroy()
    {
        UnbindGameManager();
        PauseManager.PauseChanged -= OnPauseChanged;
    }

    public void BindGameManager()
    {
        GameManager game = GameManager.Instance;
        if (game == null || subscribedGame == game) return;
        UnbindGameManager();
        subscribedGame = game;
        subscribedGame.PositiveFeedback += OnPositive;
        subscribedGame.NegativeFeedback += OnNegative;
        subscribedGame.StateChanged += OnStateChanged;
    }

    private void UnbindGameManager()
    {
        if (subscribedGame == null) return;
        subscribedGame.PositiveFeedback -= OnPositive;
        subscribedGame.NegativeFeedback -= OnNegative;
        subscribedGame.StateChanged -= OnStateChanged;
        subscribedGame = null;
    }

    private void Update()
    {
        if (subscribedGame == null) BindGameManager();
        GameManager game = subscribedGame;
        if (game == null || scoreText == null) return;

        scoreText.text = $"SCORE  {game.CurrentScore:0000}";
        int remaining = Mathf.CeilToInt(game.RemainingTime);
        timeText.text = $"TIME  {remaining / 60:00}:{remaining % 60:00}";
        timeText.color = remaining <= 7 ? new Color32(255, 93, 71, 255) : Color.white;
        livesText.text = $"LIVES  {game.CurrentLives}";
        footballText.text = game.FootballCharges > 0
            ? $"FOOTBALL x{game.FootballCharges}  [SPACE]"
            : "FOOTBALL  --";
        footballText.color = game.FootballCharges > 0 ? ManningUIFactory.Gold : new Color(1f, 1f, 1f, 0.68f);
    }

    public void TogglePause() => PauseManager.Instance?.TogglePause();

    public void ResumeGame() => PauseManager.Instance?.ResumeGame();

    public void OpenWebsite() => Application.OpenURL(ManningContestData.WebsiteUrl);

    public void PlayAgain() => SceneManager.LoadScene("GameScene");

    public void ChangeCharacter() => SceneManager.LoadScene("CharacterSelectScene");

    public void LoadMainMenu()
    {
        PauseManager.Instance?.ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPauseChanged(bool paused)
    {
        if (pauseOverlay != null && !endShown) pauseOverlay.SetActive(paused);
    }

    private void OnStateChanged(GameManager.GameState state) => ShowEnd(state);

    private void OnPositive(string message) => ShowToast(message, ManningUIFactory.Gold);

    private void OnNegative(string message) => ShowToast(message, new Color32(255, 106, 81, 255));

    private void ShowToast(string message, Color color)
    {
        if (toastText == null || endShown) return;
        toastText.text = message;
        toastText.color = color;
        toastText.gameObject.SetActive(true);
        if (toastRoutine != null) StopCoroutine(toastRoutine);
        toastRoutine = StartCoroutine(HideToast());
    }

    private IEnumerator HideToast()
    {
        yield return new WaitForSecondsRealtime(2.2f);
        if (toastText != null) toastText.gameObject.SetActive(false);
    }

    private void ShowEnd(GameManager.GameState state)
    {
        if (endShown || state == GameManager.GameState.Playing || subscribedGame == null) return;
        endShown = true;
        Time.timeScale = 1f;
        if (pauseOverlay != null) pauseOverlay.SetActive(false);
        ManningContestData.RecordScore(subscribedGame.CurrentScore, CharacterSelection.SelectedCharacter);

        endOverlay.SetActive(true);
        endArtwork.texture = ManningAssetLibrary.LoadTexture(
            state == GameManager.GameState.Won ? ManningAssetLibrary.WinScreen : ManningAssetLibrary.LoseScreen);
        AspectRatioFitter fitter = endArtwork.GetComponent<AspectRatioFitter>();
        if (fitter != null && endArtwork.texture != null)
            fitter.aspectRatio = endArtwork.texture.width / (float)endArtwork.texture.height;
        if (finalScoreText != null) finalScoreText.text = $"FINAL SCORE  {subscribedGame.CurrentScore:0000}";
        if (leaderboardText != null) leaderboardText.text = ManningContestData.GetLeaderboardText();
    }

    private void BuildRuntimeHierarchy()
    {
        canvas = ManningUIFactory.CreateCanvas("ManningGameCanvas (Runtime Fallback)", 700);
        BuildRuntimeHud();
        BuildRuntimePause();
        BuildRuntimeEnd();
    }

    private void BuildRuntimeHud()
    {
        GameObject bar = new GameObject("Top HUD", typeof(RectTransform), typeof(Image));
        bar.transform.SetParent(canvas.transform, false);
        RectTransform rect = bar.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = new Vector2(0f, 76f);
        bar.GetComponent<Image>().color = new Color(0.01f, 0.06f, 0.14f, 0.92f);

        scoreText = ManningUIFactory.CreateText(bar.transform, "Score", "SCORE  0000", 28, TextAnchor.MiddleLeft,
            new Vector2(0f, 0.5f), new Vector2(280f, 60f), new Vector2(160f, 0f), Color.white);
        timeText = ManningUIFactory.CreateText(bar.transform, "Time", "TIME  00:30", 30, TextAnchor.MiddleCenter,
            new Vector2(0.36f, 0.5f), new Vector2(280f, 60f), Vector2.zero, Color.white);
        livesText = ManningUIFactory.CreateText(bar.transform, "Lives", "LIVES  3", 28, TextAnchor.MiddleCenter,
            new Vector2(0.59f, 0.5f), new Vector2(300f, 60f), Vector2.zero, new Color32(255, 126, 71, 255));
        footballText = ManningUIFactory.CreateText(bar.transform, "Football", "FOOTBALL  --", 23, TextAnchor.MiddleCenter,
            new Vector2(0.79f, 0.5f), new Vector2(340f, 60f), Vector2.zero, ManningUIFactory.Gold);
        ManningUIFactory.CreateButton(bar.transform, "Pause", "PAUSE", new Vector2(1f, 0.5f), new Vector2(150f, 54f),
            new Vector2(-95f, 0f), ManningUIFactory.Blue, TogglePause, 22);

        toastText = ManningUIFactory.CreateText(canvas.transform, "Feedback", string.Empty, 29, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 1f), new Vector2(1000f, 62f), new Vector2(0f, -112f), Color.white);
        toastText.gameObject.SetActive(false);
    }

    private void BuildRuntimePause()
    {
        pauseOverlay = ManningUIFactory.CreatePanel(canvas.transform, "Pause Overlay", new Color(0f, 0f, 0f, 0.82f));
        ManningUIFactory.CreateText(pauseOverlay.transform, "Paused", "PAUSED", 76, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(700f, 110f), new Vector2(0f, 160f), ManningUIFactory.Gold);
        ManningUIFactory.CreateButton(pauseOverlay.transform, "Resume", "RESUME", new Vector2(0.5f, 0.5f), new Vector2(300f, 78f),
            new Vector2(0f, -30f), ManningUIFactory.Orange, ResumeGame);
        ManningUIFactory.CreateButton(pauseOverlay.transform, "Main Menu", "MAIN MENU", new Vector2(0.5f, 0.5f), new Vector2(300f, 70f),
            new Vector2(0f, -130f), ManningUIFactory.Blue, LoadMainMenu, 27);
        pauseOverlay.SetActive(false);
    }

    private void BuildRuntimeEnd()
    {
        endOverlay = new GameObject("End Overlay", typeof(RectTransform));
        endOverlay.transform.SetParent(canvas.transform, false);
        ManningUIFactory.Stretch(endOverlay.GetComponent<RectTransform>());
        endArtwork = ManningUIFactory.CreateScreen(endOverlay.transform, ManningAssetLibrary.WinScreen, Color.white);
        finalScoreText = ManningUIFactory.CreateText(endOverlay.transform, "Final Score", "FINAL SCORE  0000", 46,
            TextAnchor.MiddleCenter, new Vector2(0.5f, 0f), new Vector2(720f, 70f), new Vector2(0f, 205f), ManningUIFactory.Gold);
        leaderboardText = ManningUIFactory.CreateText(endOverlay.transform, "Local Top Scores", string.Empty, 25,
            TextAnchor.UpperLeft, new Vector2(0.82f, 0.5f), new Vector2(340f, 240f), Vector2.zero, Color.white);
        ManningUIFactory.CreateButton(endOverlay.transform, "Again", "PLAY AGAIN", new Vector2(0.5f, 0f), new Vector2(270f, 70f),
            new Vector2(-290f, 100f), ManningUIFactory.Orange, PlayAgain, 26);
        ManningUIFactory.CreateButton(endOverlay.transform, "Character", "CHANGE MANNING", new Vector2(0.5f, 0f), new Vector2(300f, 70f),
            new Vector2(0f, 100f), ManningUIFactory.Blue, ChangeCharacter, 24);
        ManningUIFactory.CreateButton(endOverlay.transform, "Menu", "MAIN MENU", new Vector2(0.5f, 0f), new Vector2(250f, 70f),
            new Vector2(285f, 100f), new Color32(46, 56, 76, 250), LoadMainMenu, 25);
        endOverlay.SetActive(false);
    }
}
