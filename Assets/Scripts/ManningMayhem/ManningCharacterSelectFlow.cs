using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Controls the hierarchy-authored character selection screen.</summary>
public sealed class ManningCharacterSelectFlow : MonoBehaviour
{
    [SerializeField] private Font brandFont;
    [SerializeField] private Canvas authoredCanvas;

    public void ConfigureHierarchy(Font font, Canvas canvas)
    {
        brandFont = font;
        authoredCanvas = canvas;
    }

    private void Awake()
    {
        ManningUIFactory.SetFontFamily(brandFont);
    }

    private void Start()
    {
        if (authoredCanvas != null) return;

        Canvas canvas = ManningUIFactory.CreateCanvas("ManningCharacterSelectCanvas (Runtime Fallback)", 600);
        ManningUIFactory.CreateScreen(canvas.transform, ManningAssetLibrary.CharacterSelectScreen, Color.white);
        ManningUIFactory.CreateButton(canvas.transform, "Peyton", "PLAY AS PEYTON", new Vector2(0.5f, 0f),
            new Vector2(360f, 78f), new Vector2(-310f, 72f), ManningUIFactory.Orange, SelectPeyton, 28);
        ManningUIFactory.CreateButton(canvas.transform, "Eli", "PLAY AS ELI", new Vector2(0.5f, 0f),
            new Vector2(360f, 78f), new Vector2(310f, 72f), ManningUIFactory.Blue, SelectEli, 28);
    }

    public void SelectPeyton() => Select(CharacterSelection.Character.Peyton);

    public void SelectEli() => Select(CharacterSelection.Character.Eli);

    public void BackToMenu() => SceneManager.LoadScene("MainMenu");

    private static void Select(CharacterSelection.Character character)
    {
        CharacterSelection.SelectedCharacter = character;
        PlayerPrefs.SetInt("Manning.SelectedCharacter", (int)character);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
