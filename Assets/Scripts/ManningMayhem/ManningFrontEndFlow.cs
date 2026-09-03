using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the scene-authored Title -> Entry -> Overview -> Rules flow. Every production panel,
/// field, label, and button remains visible and editable in the MainMenu hierarchy.
/// </summary>
public sealed class ManningFrontEndFlow : MonoBehaviour
{
    [Header("Client Font")]
    [SerializeField] private Font brandFont;

    [Header("Hierarchy Screens")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject entryScreen;
    [SerializeField] private GameObject overviewScreen;
    [SerializeField] private GameObject rulesScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject termsOverlay;

    [Header("Player Entry Fields")]
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField phoneField;
    [SerializeField] private Text validationText;

    [Header("Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private Canvas fallbackCanvas;

    public bool IsHierarchyConfigured => titleScreen != null && entryScreen != null && overviewScreen != null &&
        rulesScreen != null && settingsScreen != null && nameField != null && emailField != null && phoneField != null;

    public void ConfigureHierarchy(Font font, GameObject title, GameObject entry, GameObject overview,
        GameObject rules, GameObject settings, GameObject terms, InputField playerName, InputField email,
        InputField phone, Text validation, Slider music, Slider sfx)
    {
        brandFont = font;
        titleScreen = title;
        entryScreen = entry;
        overviewScreen = overview;
        rulesScreen = rules;
        settingsScreen = settings;
        termsOverlay = terms;
        nameField = playerName;
        emailField = email;
        phoneField = phone;
        validationText = validation;
        musicSlider = music;
        sfxSlider = sfx;
    }

    private void Awake()
    {
        ManningUIFactory.SetFontFamily(brandFont);
    }

    private void Start()
    {
        if (!IsHierarchyConfigured)
        {
            Debug.LogWarning("[ManningFrontEndFlow] Authored hierarchy is missing; using compact runtime fallback.");
            BuildRuntimeFallback();
            return;
        }

        ManningAudio audio = ManningAudio.Ensure();
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(audio.MusicVolume);
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(audio.SfxVolume);
        ShowTitle();
    }

    public void ShowTitle() => ShowOnly(titleScreen);

    public void ShowEntry()
    {
        if (validationText != null) validationText.text = string.Empty;
        ShowOnly(entryScreen);
        if (nameField != null) nameField.Select();
    }

    public void ShowOverview() => ShowOnly(overviewScreen);

    public void ShowRules() => ShowOnly(rulesScreen);

    public void ShowSettings() => ShowOnly(settingsScreen);

    public void SubmitEntry()
    {
        if (!IsHierarchyConfigured) return;

        string playerName = nameField.text?.Trim();
        string email = emailField.text?.Trim();
        string phone = phoneField.text?.Trim();
        bool validEmail = !string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.LastIndexOf('.') > email.IndexOf('@');
        if (string.IsNullOrWhiteSpace(playerName) || !validEmail || string.IsNullOrWhiteSpace(phone))
        {
            validationText.text = "ENTER NAME, VALID EMAIL, AND PHONE - OR CHOOSE SKIP";
            return;
        }

        validationText.text = string.Empty;
        ManningContestData.SaveEntry(playerName, email, phone);
        ShowOverview();
    }

    public void SkipEntry()
    {
        ManningContestData.SkipEntry();
        ShowOverview();
    }

    public void ShowTerms()
    {
        if (!string.IsNullOrWhiteSpace(ManningContestData.TermsUrl))
        {
            Application.OpenURL(ManningContestData.TermsUrl);
            return;
        }

        if (termsOverlay != null) termsOverlay.SetActive(true);
    }

    public void CloseTerms()
    {
        if (termsOverlay != null) termsOverlay.SetActive(false);
    }

    public void SetMusicVolume(float value) => ManningAudio.Ensure().SetMusicVolume(value);

    public void SetSfxVolume(float value) => ManningAudio.Ensure().SetSfxVolume(value);

    public void OpenWebsite() => Application.OpenURL(ManningContestData.WebsiteUrl);

    public void LoadCharacterSelect() => SceneManager.LoadScene("CharacterSelectScene");

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowOnly(GameObject target)
    {
        GameObject[] screens = { titleScreen, entryScreen, overviewScreen, rulesScreen, settingsScreen };
        foreach (GameObject screen in screens)
        {
            if (screen != null) screen.SetActive(screen == target);
        }
        if (termsOverlay != null) termsOverlay.SetActive(false);
    }

    private void BuildRuntimeFallback()
    {
        fallbackCanvas = ManningUIFactory.CreateCanvas("ManningFrontEndCanvas (Runtime Fallback)", 600);
        RawImage art = ManningUIFactory.CreateScreen(fallbackCanvas.transform, ManningAssetLibrary.TitleScreen, Color.white);
        ManningUIFactory.CreateArtButton(art.transform, "Play", "PLAY", 884f, 498f, 236f, 48f,
            ManningUIFactory.MenuPlate, BuildRuntimeEntry, 30);
        ManningUIFactory.CreateArtButton(art.transform, "Rules", "RULES", 884f, 692f, 236f, 48f,
            ManningUIFactory.MenuPlate, LoadCharacterSelect, 30);
    }

    private void BuildRuntimeEntry()
    {
        ManningUIFactory.Clear(fallbackCanvas.transform);
        GameObject background = ManningUIFactory.CreatePanel(fallbackCanvas.transform, "Player Entry", ManningUIFactory.Navy);
        ManningUIFactory.CreateText(background.transform, "Title", "PLAYER INFORMATION", 52, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(900f, 80f), new Vector2(0f, 310f), ManningUIFactory.Gold,
            ManningUIFactory.DisplayFont);
        nameField = ManningUIFactory.CreateInputField(background.transform, "Name", "NAME", new Vector2(0f, 160f));
        emailField = ManningUIFactory.CreateInputField(background.transform, "Email", "EMAIL", new Vector2(0f, 55f), InputField.ContentType.EmailAddress);
        phoneField = ManningUIFactory.CreateInputField(background.transform, "Phone", "PHONE", new Vector2(0f, -50f));
        validationText = ManningUIFactory.CreateText(background.transform, "Validation", string.Empty, 20, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), new Vector2(900f, 48f), new Vector2(0f, -115f), ManningUIFactory.Orange);
        ManningUIFactory.CreateButton(background.transform, "Submit", "SUBMIT", new Vector2(0.5f, 0.5f), new Vector2(260f, 72f),
            new Vector2(-150f, -195f), ManningUIFactory.Orange, SubmitEntry, 28);
        ManningUIFactory.CreateButton(background.transform, "Skip", "SKIP", new Vector2(0.5f, 0.5f), new Vector2(220f, 72f),
            new Vector2(150f, -195f), ManningUIFactory.Blue, () =>
            {
                ManningContestData.SkipEntry();
                LoadCharacterSelect();
            }, 28);
    }
}
